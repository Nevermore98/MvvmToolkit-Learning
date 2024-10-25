using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ObservableValidatorDemo.Properties;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows;

namespace ObservableValidatorDemo.ViewModels
{

    partial class MainWindowVM : ObservableValidator
    {
        [ObservableProperty]
        [Required(ErrorMessage = "用户名不能为空（此处不受语言影响）")]
        [MaxLength(14, ErrorMessageResourceName = "Username_maxLength", ErrorMessageResourceType = typeof(Lang))]
        private string _username;

        partial void OnUsernameChanged(string? value)
        {
            ValidateProperty(value, nameof(Username));
        }

        [ObservableProperty]
        [Required(ErrorMessageResourceName = "Password_required", ErrorMessageResourceType = typeof(Lang))]
        [MinLength(6, ErrorMessageResourceName ="Password_minLength", ErrorMessageResourceType =typeof(Lang))]
        [MaxLength(16, ErrorMessageResourceName = "Password_maxLength", ErrorMessageResourceType = typeof(Lang))]
        private string _password;

        partial void OnPasswordChanged(string? value)
        {
            ValidateProperty(value, nameof(Password));
        }


        [ObservableProperty]
        private string _passwordErrorMsg;

        [ObservableProperty]
        private string _usernameErrorMsg;


        public List<CultureInfo> CultureList { get; } = new List<CultureInfo>
        {
            new CultureInfo("zh-CN"),
            new CultureInfo("en-US")
        };


        // CurrentCulture 不使用 [ObservableProperty]，因为需要在修改时同步修改 I18NExtension.Culture
        private CultureInfo _currentCulture = new CultureInfo("zh-CN");
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (SetProperty(ref _currentCulture, value))
                {
                    I18NExtension.Culture = value;
                }
            }
        }

        [RelayCommand]
        private void SwitchLanguage()
        {
            // 因为 I18NExtension.Culture 只有 setter，需要一个变量保存当前语言
            if (CurrentCulture.Name == "en-US")
            {
                I18NExtension.Culture = new CultureInfo("zh-CN");
                CurrentCulture = new CultureInfo("zh-CN");
            }
            else
            {
                I18NExtension.Culture = new CultureInfo("en-US");
                CurrentCulture = new CultureInfo("en-US");
            }

            // 切换语言时，校验文本不会更新。需要清除校验，再重新校验，才会更新
            // 如果用户已经输入才重新校验，没有输入则清除校验
            bool isUserInput = !string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Password);
            if (isUserInput)
            {
                ClearErrors();
                ValidateAllProperties();
            }
            else
            {
                ClearErrors();
            }
        }


        [RelayCommand]
        private async Task LoginAsync(CancellationToken token)
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                UsernameErrorMsg = string.Join("\n", GetErrors(nameof(Username)));
                PasswordErrorMsg = string.Join("\n", GetErrors(nameof(Password)));

                return;
            }

            try
            {
                await Task.Delay(1000, token);
                // TODO
                try
                {
                }
                catch
                {
                }

            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
