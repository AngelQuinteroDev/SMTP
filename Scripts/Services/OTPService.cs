using System;

public class OTPService
{
    private string generatedOTP;
    private DateTime expirationTime;
    private int expirationMinutes = 2;

    public string GenerateOTP()
    {
        Random random = new Random();
        generatedOTP = random.Next(100000, 999999).ToString();
        expirationTime = DateTime.Now.AddMinutes(expirationMinutes);
        return generatedOTP;
    }

    public bool ValidateOTP(string inputOTP)
    {
        if (DateTime.Now > expirationTime)
        {
            return false;
        }

        return inputOTP == generatedOTP;
    }
}