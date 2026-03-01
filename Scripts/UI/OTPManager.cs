using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OTPManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField otpInput;
    public TMP_Text statusText;
    public Button startGameButton;

    private OTPService otpService;
    private EmailService emailService;

    private string fromEmail = "angel.pruebas.servicios@gmail.com";
    private string password = "jkir gvdc ztah tjdj";

    void Start()
    {
        startGameButton.interactable = false;

        otpService = new OTPService();
        emailService = new EmailService(fromEmail, password);
    }

public void SendOTP()
{
    string generatedOTP = otpService.GenerateOTP();

    string subject = "Tu código OTP";
    string body = "Tu código es: " + generatedOTP;

    EmailResult result = emailService.SendEmail(emailInput.text, subject, body);

    if (result.Success)
    {
        statusText.text = "Éxito (" + result.StatusCode + ")";
    }
    else
    {
        statusText.text = "Error (" + result.StatusCode + ")";
    }
}

    public void ValidateOTP()
    {
        bool isValid = otpService.ValidateOTP(otpInput.text);

        if (isValid)
        {
            statusText.text = "Código correcto";
            startGameButton.interactable = true;
        }
        else
        {
            statusText.text = "Código incorrecto o expirado";
        }
    }
}