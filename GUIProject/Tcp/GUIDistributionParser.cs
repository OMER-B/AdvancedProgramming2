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
        private TACHolder holder;
        private MessageTypeEnum type;
        private ObservableCollection<TitleAndContent> list;
        public GUIDistributionParser(string json, SettingsModel settm, IModel logm)
        {
            this.settingModel = settm;
            this.logModel = logm;
            this.json = json;
        }

        private void ParseJSON()
        {
            this.holder = new TACHolder(JsonConvert.DeserializeObject<TACHolder>(this.json));
            this.type = this.holder.CommandID;
            this.list = new ObservableCollection<TitleAndContent>(this.holder.List);
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
            ParseJSON();
            switch (this.type)
            {
                case MessageTypeEnum.SEND_CONFIG:
                    JSONIsConfig();
                    break;
                case MessageTypeEnum.SEND_HISTORY:
                    JSONIsHistory();
                    break;
                case MessageTypeEnum.SEND_LOG:
                case MessageTypeEnum.REQ_HISTORY:
                    JSONIsHistory();
                    break;
                case MessageTypeEnum.SUCCESS:
                case MessageTypeEnum.FAIL:
                    break;
                case MessageTypeEnum.DISCONNECT:
                    break;
            }
        }
    }
}
