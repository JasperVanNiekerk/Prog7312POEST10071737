using Prog7312POEST10071737.Services;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Prog7312POEST10071737.Views
{
    /// <summary>
    /// Interaction logic for LoginRegisterView.xaml
    /// </summary>
    public partial class LoginRegisterView : UserControl
    {
        /// <summary>
        /// The email service used for sending emails.
        /// </summary>
        private readonly MyEmailService _emailService;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The user service for managing user data.
        /// </summary>
        private readonly UserSingleton _userService;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The current OTP (One-Time Password) generated for login.
        /// </summary>
        private string _currentOtp;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The expiry date and time of the current OTP.
        /// </summary>
        private DateTime _otpExpiry;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The timer used to update the OTP expiry time.
        /// </summary>
        private DispatcherTimer _otpTimer;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The validity period of the OTP in minutes.
        /// </summary>
        private const int OTP_VALIDITY_MINUTES = 5;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Delegate for the LoginSuccessful event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void LoginSuccessfulEventHandler(object sender, EventArgs e);
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event raised when the login is successful.
        /// </summary>
        public event LoginSuccessfulEventHandler LoginSuccessful;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the LoginRegisterView class.
        /// </summary>
        public LoginRegisterView()
        {
            InitializeComponent();
            _emailService = new MyEmailService();
            _userService = UserSingleton.Instance;
            InitializeOtpTimer();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes the OTP timer.
        /// </summary>
        private void InitializeOtpTimer()
        {
            _otpTimer = new DispatcherTimer();
            _otpTimer.Interval = TimeSpan.FromSeconds(1);
            _otpTimer.Tick += OtpTimer_Tick;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the OTP timer tick event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the input field text changed event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void InputField_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Validates the input fields and updates the UI accordingly.
        /// </summary>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the validation error message based on the current input values.
        /// </summary>
        /// <returns>The validation error message.</returns>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the Send OTP button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the Verify OTP button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the Register button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Generates a random OTP (One-Time Password).
        /// </summary>
        /// <returns>The generated OTP.</returns>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Checks if the given email address is valid.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email address is valid, otherwise false.</returns>
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
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Raises the LoginSuccessful event.
        /// </summary>
        private void OnLoginSuccessful()
        {
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the OTP text box text changed event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OtpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Logic to handle OTP text change
            VerifyOtpButton.IsEnabled = !string.IsNullOrWhiteSpace(OtpTextBox.Text);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Event handler for the Resend OTP button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void ResendOtpButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to handle resending OTP
            SendOtpButton_Click(sender, e);
        }
    }
}
//____________________________________EOF_________________________________________________________________________