using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoChat.Models;

namespace WoChat.ViewModels {

    public class ContactViewModel {
        public ObservableCollection<ContactModel> Contacts {
            get {
                return contacts;
            }
        }

        public void Load() {
            // TODO: Load from db
            ContactModel cm1 = new ContactModel();
            cm1.Remark = "HH1";
            Contacts.Add(cm1);
            ContactModel cm2 = new ContactModel();
            cm2.Remark = "HH2";
            Contacts.Add(cm2);
        }

        private ObservableCollection<ContactModel> contacts = new ObservableCollection<ContactModel>();
    }
}
