using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace MvvmToolkitDemo01.ViewModels
{
    public partial class MainWindowVM : ObservableValidator
    {
        [ObservableProperty]
        [RegularExpression(@"^-?\d+$")]
        [NotifyPropertyChangedFor(nameof(DisplayResult))]
        [NotifyCanExecuteChangedFor(nameof(CalculateCommand))] // 属性的值发生变化后通知中继指令更新状态
        string value1;

        [ObservableProperty]
        [RegularExpression(@"^-?\d+$")]
        [NotifyPropertyChangedFor(nameof(DisplayResult))]
        [NotifyCanExecuteChangedFor(nameof(CalculateCommand))] // 属性的值发生变化后通知中继指令更新状态
        string value2;

         
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayResult))]
        string result;

        public string DisplayResult => !string.IsNullOrEmpty(Value1) && !string.IsNullOrEmpty(Value2) ? $"{Value1} + {Value2} = {Result}" : string.Empty;


        [ObservableProperty]
        string status;

        public bool CanCalculate => !string.IsNullOrEmpty(Value1) && ! string.IsNullOrEmpty(Value2);

        [ObservableProperty]
        string errMessage;

        // 同步指令
        //[RelayCommand(CanExecute = nameof(CanCalculate))]
        //private void Calculate()
        //{
        //    try
        //    {
        //        Result = $"{int.Parse(Value1) + int.Parse(Value2)}";
        //    }
        //    catch
        //    {
        //        Result = "Error!";
        //    }
        //    OnPropertyChanged(nameof(DisplayResult));
        //}

        // 异步指令
        [RelayCommand(CanExecute =nameof(CanCalculate), IncludeCancelCommand = true)]
        private async Task CalculateAsync(CancellationToken token)
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                //ErrMessage = GetErrors();
                var a = GetErrors();
            }
            try
            {
                Status = "Calculating...";
                Result = "";
                await Task.Delay(2000, token);
                // TODO
                try
                {
                    Result = $"{int.Parse(Value1) + int.Parse(Value2)}";
                }
                catch
                {
                    Result = "Error!";
                }
                Status = "Done";
            }
            catch (OperationCanceledException)
            {
                Result = "";
                var messagebox = new MessageBox();
                Status = "Canceled";
            }
            OnPropertyChanged(nameof(DisplayResult));
        }

    }
}
