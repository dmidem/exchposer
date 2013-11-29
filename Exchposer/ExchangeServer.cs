using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Exchange.WebServices.Data;

namespace Exchposer
{
    public class ExchangeServer
    {
        const int findPageSize = 100;

        private string exchangeUserName = null;
        private string exchangePassword = null;
        private string exchangeDomain = null;
        private string exchangeUrl = null;

        private ExchangeService service = null;

        private StreamingSubscriptionConnection subscriptionConnection = null;
        private StreamingSubscription streamingSubscription = null;
        private Action<EmailMessage> onReceive = null;

        private readonly Action<int, string> logger = null;

        protected void Log(int level, string message)
        {
            if (logger != null)
                logger(level, message);
        }

        public ExchangeServer(string exchangeUserName, string exchangePassword, string exchangeDomain, string exchangeUrl, Action<int, string> logger = null)
        {
            this.logger = logger;
            this.exchangeUserName = exchangeUserName;
            this.exchangePassword = exchangePassword;
            this.exchangeDomain = exchangeDomain;
            this.exchangeUrl = exchangeUrl;
        }

        public void Open()
        {
            try
            {
                Close();

                service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
                service.Credentials = new WebCredentials(exchangeUserName, exchangePassword, exchangeDomain);
                service.Url = new Uri(exchangeUrl);

                Log(2, "Exchange server opened");
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange server open error: {0}", ex.Message));
                service = null;

                throw;
            }
        }

        public void Close()
        {
            ClearStreamingNotifications();

            if (service != null)
                Log(2, "Exchange server closed");
            service = null;
        }

        
        public void ProcessMessages(DateTime fromTime, DateTime toTime, Action<EmailMessage> messageAction)
        {
            Folder inbox = Folder.Bind(service, WellKnownFolderName.Inbox);

            SearchFilter sf = new SearchFilter.SearchFilterCollection(LogicalOperator.And,
                new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeReceived, fromTime),
                new SearchFilter.IsLessThanOrEqualTo(EmailMessageSchema.DateTimeReceived, toTime));

            for (int findOffset = 0; ; findOffset += findPageSize)
            {
                ItemView view = new ItemView(findPageSize, findOffset, OffsetBasePoint.Beginning); ;
                view.PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.DateTimeReceived);
                view.OrderBy.Add(EmailMessageSchema.DateTimeReceived, SortDirection.Ascending);

                FindItemsResults<Item> findResults = service.FindItems(WellKnownFolderName.Inbox, sf, view);

                foreach (EmailMessage msg in findResults)
                    if (messageAction != null)
                        messageAction(msg);

                if (!findResults.MoreAvailable)
                    break;
            }
        }


        public void LoadMessage(EmailMessage msg)
        {
            msg.Load(new PropertySet(new PropertyDefinitionBase[] {
                    EmailMessageSchema.DateTimeReceived,
                    EmailMessageSchema.Id,
                    EmailMessageSchema.InternetMessageId,
                    EmailMessageSchema.MimeContent,
                    EmailMessageSchema.From,
                    EmailMessageSchema.Sender,
                    EmailMessageSchema.ToRecipients,
                    EmailMessageSchema.Subject
                }));
        }

        public void SetStreamingNotifications(Action<EmailMessage> onReceive, int lifetime)
        {
            this.onReceive = onReceive;

            try
            {
                subscriptionConnection = new StreamingSubscriptionConnection(service, lifetime);
                subscriptionConnection.OnNotificationEvent +=
                    new StreamingSubscriptionConnection.NotificationEventDelegate(OnEvent);
                subscriptionConnection.OnSubscriptionError +=
                    new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnError);
                subscriptionConnection.OnDisconnect +=
                    new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnDisconnect);
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription connection creating error: {0}", ex.Message));
                ClearStreamingNotifications();
                return;
            }

            try
            {
                subscriptionConnection.AddSubscription(streamingSubscription = service.SubscribeToStreamingNotifications(
                    new FolderId[] { WellKnownFolderName.Inbox },
                    EventType.NewMail,
                    EventType.Created,
                    EventType.Deleted));
                subscriptionConnection.Open();
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription creating error: {0}", ex.Message));
                ClearStreamingNotifications();
                return;
            }

            Log(2, "Exchange subscription started");
        }

        public void ClearStreamingNotifications()
        {
            try
            {
                /*
                if (subscriptionConnection != null)
                    subscriptionConnection.RemoveSubscription(streamingSubscription);

                if (streamingSubscription != null)
                    streamingSubscription.Unsubscribe();
                streamingSubscription = null;

                if (subscriptionConnection != null)
                    subscriptionConnection.Close();
                subscriptionConnection = null;
                */

                if (subscriptionConnection != null)
                {
                    subscriptionConnection.Close();
                    subscriptionConnection.RemoveSubscription(streamingSubscription);
                }
                subscriptionConnection = null;

                if (streamingSubscription != null)
                {
                    streamingSubscription.Unsubscribe();
                }
                streamingSubscription = null;


                this.onReceive = null;
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription stopping error: {0}", ex.Message));
            }

            Log(2, "Exchange subscription stopped");
        }

        private void OnEvent(object sender, NotificationEventArgs args)
        {
            StreamingSubscription subscription = args.Subscription;

            foreach (NotificationEvent notification in args.Events)
            {
                string logInfo = (notification is ItemEvent ?
                    "ItemId: " + ((ItemEvent)notification).ItemId.UniqueId :
                    "FolderId: " + ((FolderEvent)notification).FolderId.UniqueId);

                switch (notification.EventType)
                {
                    case EventType.NewMail:
                        Log(20, String.Format("Exchange subscription event: new mail ({0})", logInfo));

                        ItemEvent itemEvent = (ItemEvent)notification;
                        EmailMessage msg = EmailMessage.Bind(service, itemEvent.ItemId, new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.DateTimeReceived));

                        if (onReceive != null)
                            onReceive(msg);

                        break;

                    case EventType.Created:
                        Log(20, String.Format("Exchange subscription event: item or folder created ({0})", logInfo));
                        break;

                    case EventType.Deleted:
                        Log(20, String.Format("Exchange subscription event: item or folder deleted ({0})", logInfo));
                        break;
                }
            }
        }

        private void OnError(object sender, SubscriptionErrorEventArgs args)
        {
            Log(3, String.Format("Exchange subscription error: {0} Resetting subscribtion...", args.Exception.Message));

            /*
            try
            {
                subscriptionConnection.Close();
                //streamingSubscription.Unsubscribe();
                //subscriptionConnection.RemoveSubscription(streamingSubscription);
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription removing error: {0}", ex.Message));
            }
            */

            try
            {
                subscriptionConnection.AddSubscription(streamingSubscription = service.SubscribeToStreamingNotifications(
                      new FolderId[] { WellKnownFolderName.Inbox },
                      EventType.NewMail,
                      EventType.Created,
                      EventType.Deleted));
                //subscriptionConnection.Open();
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription creating error: {0}", ex.Message));
                ClearStreamingNotifications();
                return;
            }

            Log(2, "Exchange subscription started");
        }

        private void OnDisconnect(object sender, SubscriptionErrorEventArgs args)
        {
            //if (streamingSubscription == null)
            //    return;

            Log(3, String.Format("Exchange subscription disconnected. Reconnecting..."));

            try
            {
                StreamingSubscriptionConnection connection = (StreamingSubscriptionConnection)sender;
                connection.Open();
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription connection open error: {0}", ex.Message));
                //ClearStreamingNotifications();
                return;
            }
        }
    }
}
