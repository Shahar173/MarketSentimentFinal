using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MarketSentimentFinal.Models;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.ViewModels
{
    public class TransactionDetailsViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private WhaleTransaction _transaction;
        public WhaleTransaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand { get; }
        public ICommand CopyTextCommand { get; }

        public TransactionDetailsViewModel()
        {
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            CopyTextCommand = new Command<string>(async (text) => await CopyToClipboardAsync(text));
        }

        // קליטת האובייקט בצורה בטוחה מהניווט
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("SelectedTransaction", out var tx) && tx is WhaleTransaction whaleTx)
            {
                Transaction = whaleTx;
            }
        }

        private async Task CopyToClipboardAsync(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                await Clipboard.Default.SetTextAsync(text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[COPY ERROR] {ex.Message}");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}