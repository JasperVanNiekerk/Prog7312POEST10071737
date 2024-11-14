using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prog7312POEST10071737.Services;
using System.Security.Cryptography;
using System.Windows.Threading;

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Interaction logic for LoginRegisterView.xaml
    /// </summary>
    public partial class LoginRegisterView : UserControl
    {
        private readonly MyEmailService _emailService;
        private readonly UserSingleton _userService;
        private string _currentOtp;
        private DateTime _otpExpiry;
        private DispatcherTimer _otpTimer;
        private const int OTP_VALIDITY_MINUTES = 5;

        // Define a delegate for the event
        public delegate void LoginSuccessfulEventHandler(object sender, EventArgs e);
        
        // Define the event using the delegate
        public event LoginSuccessfulEventHandler LoginSuccessful;

        public LoginRegisterView()
        {
            InitializeComponent();
            _emailService = new MyEmailService();
            _userService = UserSingleton.Instance;
            InitializeOtpTimer();
        }

        private void InitializeOtpTimer()
        {
            _otpTimer = new DispatcherTimer();
            _otpTimer.Interval = TimeSpan.FromSeconds(1);
            _otpTimer.Tick += OtpTimer_Tick;
        }

        private void OtpTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now >= _otpExpiry)
            {
                _otpTimer.Stop();
                _currentOtp = null;
                OtpTimerText.Text = "OTP expired";
                ResendOtpButton.Visibility = Visibility.Visible;
                VerifyOtpButton.IsEnabled = false;
            }
            else
            {
                var timeLeft = _otpExpiry - DateTime.Now;
                OtpTimerText.Text = $"OTP expires in: {timeLeft.Minutes}:{timeLeft.Seconds:D2}";
            }
        }

        private void InputField_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            var isValid = !string.IsNullOrWhiteSpace(UsernameTextBox.Text) &&
                         !string.IsNullOrWhiteSpace(EmailTextBox.Text) &&
                         IsValidEmail(EmailTextBox.Text);

            SendOtpButton.IsEnabled = isValid;
            
            if (!isValid)
            {
                StatusMessage.Text = GetValidationErrorMessage();
            }
            else
            {
                StatusMessage.Text = string.Empty;
            }
        }

        private string GetValidationErrorMessage()
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                return "Username is required";
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                return "Email is required";
            if (!IsValidEmail(EmailTextBox.Text))
                return "Please enter a valid email address";
            return string.Empty;
        }

        private async void SendOtpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendOtpButton.IsEnabled = false;
                StatusMessage.Text = "Sending OTP...";

                _currentOtp = GenerateOTP();
                _otpExpiry = DateTime.Now.AddMinutes(OTP_VALIDITY_MINUTES);

                var subject = "Your Login OTP";
                var body = $"Hello {UsernameTextBox.Text},\n\n" +
                          $"Your OTP is: {_currentOtp}\n" +
                          $"Valid for {OTP_VALIDITY_MINUTES} minutes.\n\n" +
                          "If you didn't request this OTP, please ignore this email.";

                var email = EmailTextBox.Text.Trim();
                await Task.Run(() => _emailService.EmailSender(email, subject, body));

                OtpGrid.Visibility = Visibility.Visible;
                StatusMessage.Text = "OTP sent! Please check your email.";
                StatusMessage.Foreground = Brushes.Green;

                _otpTimer.Start();
            }
            catch (Exception ex)
            {
                StatusMessage.Text = "Failed to send OTP. Please try again.";
            }
        }

        private void VerifyOtpButton_Click(object sender, RoutedEventArgs e)
        {
            var enteredOtp = OtpTextBox.Text.Trim();
            
            if (DateTime.Now > _otpExpiry)
            {
                StatusMessage.Text = "OTP has expired. Please request a new one.";
                return;
            }

            if (enteredOtp == _currentOtp)
            {
                var email = EmailTextBox.Text.Trim();
                try
                {
                    if (!_userService.UserExists())
                    {
                        _userService.CreateUser("", "", email);
                    }
                    
                    var user = new Models.User("", "", email);
                    _userService.Login(user);
                    
                    StatusMessage.Text = "Login successful!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Green;
                    
                    // Call the method to raise the event
                    OnLoginSuccessful();
                }
                catch (Exception ex)
                {
                    StatusMessage.Text = "Failed to complete login. Please try again.";
                    return;
                }
            }
            else
            {
                StatusMessage.Text = "Invalid OTP. Please try again.";
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text.Trim();
            
            if (!IsValidEmail(email))
            {
                StatusMessage.Text = "Please enter a valid email address";
                return;
            }

            try
            {
                _userService.CreateUser("", "", email);
                
                // Send welcome email
                var subject = "Welcome to Our App";
                var body = "Thank you for registering! You can now login using OTP.";

                _emailService.EmailSender(email, subject, body);

                StatusMessage.Text = "Registration successful! You can now login.";
                StatusMessage.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                StatusMessage.Text = "Registration failed. Please try again.";
            }
        }

        private string GenerateOTP()
        {
            // Generate a 6-digit OTP
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                var random = BitConverter.ToUInt32(bytes, 0);
                return (random % 1000000).ToString("D6");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void OnLoginSuccessful()
        {
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }

        private void OtpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Logic to handle OTP text change
            VerifyOtpButton.IsEnabled = !string.IsNullOrWhiteSpace(OtpTextBox.Text);
        }

        private void ResendOtpButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to handle resending OTP
            SendOtpButton_Click(sender, e);
        }
    }
}
  