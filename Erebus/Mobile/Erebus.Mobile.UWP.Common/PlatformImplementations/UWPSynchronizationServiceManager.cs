﻿using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Erebus.Mobile.UWP.Common.PlatformImplementations
{
    public class UWPSynchronizationServiceManager : ISynchronizationServiceManager
    {
        private const string BackgroundTaskName = "SynchronizationBackgroundTask";
        private const string BackgroundTaskEntryPoint = "Erebus.Mobile.UWP.Tasks.SynchronizationBackgroundTask";

        public async void StartSynchronizationService()
        {
            var taskRegistered = false;

            //BackgroundExecutionManager.RemoveAccess();

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BackgroundTaskName)
                {
                    taskRegistered = true;
                    break;
                }
            }

            if (taskRegistered)
            {
                StopSynchronizationService();
            }

            var requestTask = await BackgroundExecutionManager.RequestAccessAsync();

            //if (requestTask != BackgroundAccessStatus.DeniedByUser &&
            //    requestTask != BackgroundAccessStatus.DeniedBySystemPolicy)
            //{
            var builder = new BackgroundTaskBuilder();
            builder.Name = BackgroundTaskName;
            builder.TaskEntryPoint = BackgroundTaskEntryPoint;
            builder.SetTrigger(new TimeTrigger(45, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.Register();
            //}

        }

        public void StopSynchronizationService()
        {
            BackgroundExecutionManager.RemoveAccess();
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == BackgroundTaskName)
                {
                    cur.Value.Unregister(true);
                }
            }
        }
    }
}
