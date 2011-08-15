/*
Copyright 2011 Vidar Kongsli

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0
	
Unless required by applicable law or agreed to in writing, software 
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
 */
using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Demo.Log4Net.Azure
{
    public class LogEntry : TableServiceEntity
    {
        public LogEntry()
        {
            var now = DateTime.UtcNow;
            PartitionKey = string.Format("{0:yyyy-MM}", now);
            RowKey = string.Format("{0:dd HH:mm:ss.fff}-{1}", now, Guid.NewGuid());
        }

        #region Table columns
        public string Message { get; set; }
        public string Level { get; set; }
        public string LoggerName { get; set; }
        public string Domain { get; set; }
        public string ThreadName { get; set; }
        public string Identity { get; set; }
        public string RoleInstance { get; set; }
        public string DeploymentId { get; set; }
        #endregion
    }
}
