using CommunicationTools;
using GUIProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Tcp
{
    class GUIDistributionParser
    {
        private IModel logModel;
        private SettingsModel settingModel;
        private string json;
        private ObservableCollection<TitleAndContent> list;

        public GUIDistributionParser(string json, SettingsModel settm, IModel logm)
        {
            this.settingModel = settm;
            this.logModel = logm;
            this.json = json;
        }

        private void JSONIsConfig()
        {
            foreach(TitleAndContent tac in this.list)
            {
                switch (tac.Title)
                {
                    case "Path":
                        this.settingModel.HandlersList.Add(tac);
                        break;
                    default:
                        this.settingModel.List.Add(tac);
                        break;
                }
            }
        }

        private void JSONIsHistory()
        {
            foreach (TitleAndContent tac in this.list)
            {
                this.logModel.List.Add(tac);
            }
        }

        private void JSONIsLog()
        {
            foreach (TitleAndContent tac in this.list)
            {
                this.logModel.List.Add(tac);
            }
        }

        public void passToModel()
        {
            TACHolder holder = MessageParser.ParseJsonToTAC(json);
            this.list = new ObservableCollection<TitleAndContent>(holder.List);
            switch (holder.CommandID)
            {
                case MessageTypeEnum.APP_CONFIG:
                    JSONIsConfig();
                    break;
                case MessageTypeEnum.LOG_HISTORY:
                    JSONIsHistory();
                    break;
                case MessageTypeEnum.SEND_LOG:

                case MessageTypeEnum.SUCCESS:
                case MessageTypeEnum.FAIL:
                    break;
                case MessageTypeEnum.DISCONNECT:
                    break;
            }
        }
    }
}
