using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoChat.Models;

namespace WoChat.ViewModels {
    public class MixedContactViewModel {
        public ObservableCollection<MixedContactModel> MixedContacts {
            get {
                return mixedContacts;
            }
        }

        public void Load() {
            LoadAndMix();
        }

        /// <summary>
        /// Load needed data and mix them.
        /// </summary>
        private void LoadAndMix() {
            contactVM.Load();
            groupVM.Load();
            // TODO: Mix
        }

        private ObservableCollection<MixedContactModel> mixedContacts = new ObservableCollection<MixedContactModel>();
        private ContactViewModel contactVM = new ContactViewModel();
        private GroupViewModel groupVM = new GroupViewModel();
    }
}
