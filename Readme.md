# SMTP Workshop – Email Notification Integration in Unity

## Overview

This project demonstrates the integration of an email notification system in Unity using the SMTP protocol. The implementation focuses on proper architectural separation between UI, business logic, and infrastructure layers.

## Event that Triggers the Notification

The notification is triggered when a user requests an OTP (One-Time Password) code to start the game.

### User Flow

| Step | Action | Component | Result |
|------|--------|-----------|--------|
| 1 | User enters email address | `emailInput` (TMP_InputField) | Email stored in input field |
| 2 | User clicks "Send OTP" button | `SendOTP()` method | Triggers OTP generation |
| 3 | System generates 6-digit code | `OTPService.GenerateOTP()` | Random code created (100000-999999) |
| 4 | Email is constructed | `OTPManager` | Subject and body prepared |
| 5 | Email sent via SMTP | `EmailService.SendEmail()` | Message transmitted to server |
| 6 | Status displayed to user | `statusText` (TMP_Text) | Success or error message shown |
| 7 | User receives email | Mail server | OTP code delivered |
| 8 | User enters received code | `otpInput` (TMP_InputField) | Code ready for validation |
| 9 | User clicks "Validate" | `ValidateOTP()` method | Code checked against stored value |
| 10 | Game access granted/denied | `startGameButton` state | Button enabled if valid |

**Key Implementation Details:**

```csharp
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
```

The OTP code is dynamically generated on each request and expires after 2 minutes.

## Basic SMTP Sending Flow

The email sending process is handled by the `EmailService` class and follows these steps:

### Flow Diagram

```
1. Create MailMessage object
   - Set sender (fromEmail)
   - Set recipient (toEmail)
   - Set subject and body
   
2. Configure SmtpClient
   - Server: smtp.gmail.com
   - Port: 587
   - Enable SSL: true
   - Set credentials
   
3. Send message
   - Call smtp.Send(mail)
   - Wait for server response
   
4. Return result
   - Success: EmailResult with status 250
   - Error: EmailResult with error code and message
```

### Implementation

```csharp
public EmailResult SendEmail(string toEmail, string subject, string body)
{
    try
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = body;

        SmtpClient smtp = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true
        };

        smtp.Send(mail);
        return new EmailResult(true, 250, "Correo enviado correctamente");
    }
    catch (SmtpException smtpEx)
    {
        Debug.Log("SMTP ERROR: " + smtpEx.Message);
        int code = (int)smtpEx.StatusCode;
        return new EmailResult(false, code, smtpEx.Message);
    }
    catch (Exception ex)
    {
        Debug.Log("GENERAL ERROR: " + ex.Message);
        return new EmailResult(false, 500, "Error interno: " + ex.Message);
    }
}
```

**Configuration Parameters:**
- Protocol: SMTP
- Port: 587 (TLS)
- SSL/TLS: Enabled
- Authentication: NetworkCredential
- Server: smtp.gmail.com

## Server Response Handling

The system implements structured error handling through the `EmailResult` class to manage different SMTP server responses.

### EmailResult Structure

```csharp
public class EmailResult
{
    public bool Success;
    public int StatusCode;
    public string Message;

    public EmailResult(bool success, int statusCode, string message)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
    }
}
```

### Response Handling Logic

**Success Response:**
- Status Code: `250` (Message sent successfully)
- Returns: `EmailResult(true, 250, "Correo enviado correctamente")`

**Error Responses:**

| Status Code | Description | Cause |
|-------------|-------------|-------|
| 535 | Authentication failure | Invalid credentials or app password |
| 550 | Mailbox unavailable | Recipient email address rejected |
| 500 | Internal server error | Server-side processing error |
| Others | Various SMTP errors | Network issues, timeouts, etc. |

### Exception Handling Implementation

The service implements a two-tier exception handling strategy:

```csharp
try
{
    smtp.Send(mail);
    return new EmailResult(true, 250, "Correo enviado correctamente");
}
catch (SmtpException smtpEx)
{
    Debug.Log("SMTP ERROR: " + smtpEx.Message);
    int code = (int)smtpEx.StatusCode;
    return new EmailResult(false, code, smtpEx.Message);
}
catch (Exception ex)
{
    Debug.Log("GENERAL ERROR: " + ex.Message);
    return new EmailResult(false, 500, "Error interno: " + ex.Message);
}
```

**Handling Strategy:**

1. **SmtpException**: Captures SMTP-specific errors and extracts the status code from the exception
2. **General Exception**: Catches any other errors (network, configuration, etc.) and returns status code 500

### UI Feedback

The result is displayed to the user through the UI layer:

```csharp
EmailResult result = emailService.SendEmail(emailInput.text, subject, body);

if (result.Success)
{
    statusText.text = "Éxito (" + result.StatusCode + ")";
}
else
{
    statusText.text = "Error (" + result.StatusCode + ")";
}
```

This structured approach allows the UI to provide immediate, specific feedback based on the SMTP server's response.

## Project Architecture

The implementation follows a layered architecture pattern with clear separation of concerns:

### Architecture Layers

**UI Layer** - `OTPManager.cs`
- Manages Unity UI components (TMP_InputField, Button, TMP_Text)
- Handles user interactions through button events
- Displays success/error messages and controls game access
- Coordinates between business and infrastructure layers

**Business Logic Layer** - `OTPService.cs`
- Generates random 6-digit OTP codes using `Random.Next(100000, 999999)`
- Validates user-entered codes against stored value
- Manages code expiration (2-minute timeout)
- Tracks generation time for validation

**Infrastructure Layer** - `EmailService.cs`
- Establishes SMTP connection with Gmail servers
- Constructs and sends MailMessage objects
- Handles authentication and SSL/TLS configuration
- Returns structured responses via `EmailResult.cs`

**Data Transfer Object** - `EmailResult.cs`
- Encapsulates email operation results
- Contains success status, SMTP status code, and descriptive message
- Enables structured error handling across layers

### Class Interactions

```
User Input → OTPManager → OTPService.GenerateOTP()
                ↓
         EmailService.SendEmail()
                ↓
         SMTP Server Response
                ↓
         EmailResult Object
                ↓
         UI Status Update
```

## Folder Structure

```
Assets/
 └── Scripts/
      ├── UI/
      │   └── OTPManager.cs
      └── Services/
          ├── OTPService.cs
          ├── EmailService.cs
          └── EmailResult.cs
```

## Technologies

- Unity 2021+
- C# .NET
- System.Net.Mail library
- SMTP protocol

## Author

Angel

---

**Note:** This project was developed as part of an academic workshop to demonstrate SMTP integration in game development environments.