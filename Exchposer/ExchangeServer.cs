using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
        private int subscriptionConnectionLifetime = 0;

        private readonly Action<int, string> logger = null;

        private Timer restartTimer = new Timer();

        protected void Log(int level, string message)
        {
            if (logger != null)
                logger(level, message);
        }

        public ExchangeServer(string exchangeUserName, string exchangePassword, string exchangeDomain, string exchangeUrl,
            int restartTimeout, Action<int, string> logger = null)
        {
            this.logger = logger;
            this.exchangeUserName = exchangeUserName;
            this.exchangePassword = exchangePassword;
            this.exchangeDomain = exchangeDomain;
            this.exchangeUrl = exchangeUrl;

            restartTimer.Elapsed += new ElapsedEventHandler(restartTimer_Elapsed);
            restartTimer.Interval = restartTimeout * 1000;
            restartTimer.AutoReset = false;
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
            Log(2, "Exchange server closing...");

            StopStreamingNotifications();

            this.onReceive = null;
            this.subscriptionConnectionLifetime = 0;

            if (service != null)
                Log(2, "Exchange server closed");
            service = null;
        }

        
        public void ProcessMessages(DateTime fromTime, DateTime toTime, Action<EmailMessage> messageAction)
        {
            if (messageAction == null)
                return;

            if (service == null)
            {
                Log(2, "Exchange server is null");
                return;
            }

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

        public void StartStreamingNotifications(Action<EmailMessage> onReceive, int lifetime)
        {
            this.onReceive = onReceive;
            this.subscriptionConnectionLifetime = lifetime;

            StartStreamingNotifications();
        }

        public void StartStreamingNotifications()
        {
            if (service == null)
            {
                Log(2, "Exchange server is null");
                return;
            }

            StopStreamingNotifications();

            try
            {
                subscriptionConnection = new StreamingSubscriptionConnection(service, subscriptionConnectionLifetime);
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
                if (subscriptionConnection != null)
                    subscriptionConnection.Dispose();
                subscriptionConnection = null;
                return;
            }

            try
            {
                subscriptionConnection.AddSubscription(streamingSubscription = service.SubscribeToStreamingNotifications(
                    new FolderId[] { WellKnownFolderName.Inbox }, EventType.NewMail, EventType.Created, EventType.Deleted));
                subscriptionConnection.Open();
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription creating error: {0}", ex.Message));
                RestartStreamingNotifications();
                return;
            }

            Log(2, "Exchange subscription started");
        }

        public void StopStreamingNotifications()
        {
            restartTimer.Stop();

            try
            {
                if (streamingSubscription != null)
                {
                    var tmpStreamingSubscription = streamingSubscription;
                    streamingSubscription = null;
                    tmpStreamingSubscription.Unsubscribe();
                }

                if (subscriptionConnection != null)
                {
                    if (subscriptionConnection.IsOpen)
                        subscriptionConnection.Close();
                    subscriptionConnection.Dispose();
                    subscriptionConnection = null;
                    Log(2, "Exchange subscription stopped");
                }
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription stopping error: {0}", ex.Message));
            }
        }

        public void RestartStreamingNotifications()
        {
            StopStreamingNotifications();
            if (restartTimer.Interval != 0)
                restartTimer.Start();
        }

        private void restartTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            Log(2, "Restarting exchange streaming notification...");
            StartStreamingNotifications();
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
            if (streamingSubscription == null)
                return;

            Log(3, String.Format("Exchange subscription error: {0} Restartting subscribtion...", args.Exception.Message));
            RestartStreamingNotifications();
/*
            try
            {
                if (service != null)
                    subscriptionConnection.AddSubscription(streamingSubscription = service.SubscribeToStreamingNotifications(
                        new FolderId[] { WellKnownFolderName.Inbox }, EventType.NewMail, EventType.Created, EventType.Deleted));
                //subscriptionConnection.Open();
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription creating error: {0}", ex.Message));
                return;
            }

            Log(2, "Exchange subscription started");
 */
        }

        private void OnDisconnect(object sender, SubscriptionErrorEventArgs args)
        {
            if (streamingSubscription == null)
                return;

            Log(3, String.Format("Exchange subscription disconnected. Reconnecting..."));

            try
            {
                StreamingSubscriptionConnection connection = (StreamingSubscriptionConnection)sender;
                connection.Open();
            }
            catch (Exception ex)
            {
                Log(1, String.Format("Exchange subscription connection open error: {0}", ex.Message));                
                RestartStreamingNotifications();
                return;
            }
        }
    }
}
