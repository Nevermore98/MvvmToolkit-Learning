using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace BasicDemo.ViewModels
{
    public partial class MainWindowVM : ObservableValidator
    {
        [ObservableProperty]
        [RegularExpression(@"^-?\d+$")] // 正则表达式验证
        [NotifyPropertyChangedFor(nameof(DisplayResult))] // 属性的值发生变化后通知属性变化
        [NotifyCanExecuteChangedFor(nameof(CalculateCommand))] // 属性的值发生变化后通知中继指令更新状态
        private string _value1;


        // 上面的等价写法，除了增加了 CalculateCommand.Cancel() 
        // 在等待计算时，重新输入 Value2 会取消指令
        [RegularExpression(@"^-?\d+$")]
        private string _value2;
        public string Value2
        {
            get => _value2;
            set
            {
                if (SetProperty(ref _value2, value, nameof(Value2)))
                {
                    CalculateCommand.Cancel();
                    OnPropertyChanged(nameof(DisplayResult));
                    CalculateCommand.NotifyCanExecuteChanged();
                }
            }
        }



        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayResult))]

        string result;

        public string DisplayResult => !string.IsNullOrEmpty(Value1) && !string.IsNullOrEmpty(Value2) ? $"{Value1} + {Value2} = {Result}" : string.Empty;


        [ObservableProperty]
        string status;

        public bool CanCalculate => !string.IsNullOrEmpty(Value1) && !string.IsNullOrEmpty(Value2);

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
        [RelayCommand(CanExecute = nameof(CanCalculate), IncludeCancelCommand = true)]
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
