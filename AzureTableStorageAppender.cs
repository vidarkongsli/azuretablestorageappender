using System;
using System.Data.Services.Client;
using log4net.Appender;
using log4net.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Demo.Log4Net.Azure
{
    public class AzureTableStorageAppender : AppenderSkeleton
    {
        public string TableStorageConnectionStringName { get; set; }
        private LogServiceContext _ctx;
        private string _tableEndpoint;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            var cloudStorageAccount =
                CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(TableStorageConnectionStringName));
            _tableEndpoint = cloudStorageAccount.TableEndpoint.AbsoluteUri;
            CloudTableClient.CreateTablesFromModel(typeof(LogServiceContext), _tableEndpoint, cloudStorageAccount.Credentials);
            _ctx = new LogServiceContext(cloudStorageAccount.TableEndpoint.AbsoluteUri, cloudStorageAccount.Credentials);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            Action doWriteToLog = () =>
            {
                try
                {
                    _ctx.Log(new LogEntry
                    {
                        RoleInstance = RoleEnvironment.CurrentRoleInstance.Id,
                        DeploymentId = RoleEnvironment.DeploymentId,
                        Timestamp = loggingEvent.TimeStamp,
                        Message = loggingEvent.RenderedMessage,
                        Level = loggingEvent.Level.Name,
                        LoggerName = loggingEvent.LoggerName,
                        Domain = loggingEvent.Domain,
                        ThreadName = loggingEvent.ThreadName,
                        Identity = loggingEvent.Identity
                    });
                }
                catch (DataServiceRequestException e)
                {
                    ErrorHandler.Error(string.Format("{0}: Could not write log entry to {1}: {2}",
                        GetType().AssemblyQualifiedName, _tableEndpoint, e.Message));
                }
            };
            doWriteToLog.BeginInvoke(null, null);
        }
    }
}
