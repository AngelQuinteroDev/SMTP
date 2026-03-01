# 🎮 SMTP Workshop – Email Notification Integration in Unity

## 📌 Description

This project is a mini-game developed in Unity that integrates an email notification system using the SMTP protocol.

The main objective is to demonstrate proper integration between:

- 🎮 Game logic
- 🖥 User interface (Unity UI)
- 🧠 Dynamic message generation
- 📧 Real email sending via SMTP
- 🔁 Server response handling

The focus was not on game complexity, but on the correct functional integration of the notification system.

## 🎯 Notification Trigger Event

The event that triggers the notification is:

✅ **Generation and validation of an OTP code to start the game.**

When the user:
1. Enters their email
2. Presses the button to generate the event
3. An OTP code is dynamically generated
4. An email with the code is automatically sent

## 🧱 Project Architecture

A responsibility-based separation architecture was implemented, dividing the system into three main layers:

```
UI Layer
   ↓
Business Logic Layer
   ↓
Infrastructure Layer
```

### 📂 Folder Structure

```
Assets/
 └── Scripts/
      ├── UI/
      │     └── OTPManager.cs
      ├── Services/
      │     ├── OTPService.cs
      │     ├── EmailService.cs
      │     └── EmailResult.cs
```

### 🏗 Separation of Responsibilities

| Class | Responsibility |
|-------|----------------|
| `OTPManager` | Controls the interface and flow |
| `OTPService` | Generates and validates the OTP code |
| `EmailService` | Manages SMTP communication |
| `EmailResult` | Models the server response |

## 🔄 General System Flow

### 📊 Event Diagram

```
[ User ]
     ↓
Enters email
     ↓
Presses "Send OTP"
     ↓
[ OTPManager ]
     ↓
Generates dynamic code
     ↓
[ OTPService ]
     ↓
Returns OTP
     ↓
[ EmailService ]
     ↓
SMTP Connection
     ↓
Mail server
     ↓
Response (success or error)
     ↓
[ UI displays result ]
```

## 📧 Basic SMTP Sending Flow

```
Create message (MailMessage)
        ↓
Configure SMTP server
        ↓
Authentication
        ↓
Send message
        ↓
Server responds with status code
```

The project uses:
- **Port 587**
- **SSL enabled**
- **Credential authentication**

## 🧠 Dynamic Message Generation

The subject and body of the email are dynamically constructed at runtime:

**Example:**

**Subject:** `Your OTP code to start the game`

**Body:**
```
Your code is: 483921
This code is valid for 2 minutes.
```

The OTP code changes with each execution.

## 📡 Server Response Handling

The system captures SMTP exceptions and converts them into structured objects (`EmailResult`).

**Example handling:**

- ✔ `250` → Successful sending
- ❌ `535` → Authentication error
- ❌ `550` → Invalid destination email
- ❌ `500` → Internal error

**Handling flow:**

```
SMTP Send()
    ↓
Exception?
    ↓             ↓
  NO             YES
    ↓             ↓
Return 250     Catch SmtpException
                   ↓
           Extract StatusCode
                   ↓
             Return EmailResult
                   ↓
           UI displays status
```

This allows clear visualization on screen whether the sending was successful or failed.

## 🔎 Differences from the Base Code

The provided base code performed:
- Direct sending from a single script
- No separation of responsibilities
- No server response modeling
- No logic encapsulation

### 🔁 Implemented Improvements

| Base Code | Current Implementation |
|-----------|------------------------|
| Everything in one class | Layered architecture |
| Boolean return | Structured `EmailResult` object |
| No detailed error handling | Specific `SmtpException` capture |
| Logic mixed with UI | UI / Logic / Infrastructure separation |

The new architecture allows:
- ✅ Change SMTP provider without modifying the UI
- ✅ Modify OTP logic without affecting sending
- ✅ Easily scale the project

## 🖥 User Interface

The interface allows:
- Enter destination email
- Activate the event
- View sending status
- Validate OTP code

**Visual flow:**

```
[ Email Input ]
[ Send Button ]
[ Status Message ]
[ OTP Input ]
[ Validate Button ]
```

## ✅ Requirements Met

- ✔ Mandatory use of provided SMTP code
- ✔ Real email sending
- ✔ Dynamic message construction
- ✔ Result visualization
- ✔ Integration with game event
- ✔ Server response handling
- ✔ Separation of responsibilities

## 🎥 Demo Video

(Add link here)

## 🚀 Conclusion

The project demonstrates functional integration between:
- Game logic
- User interface
- Dynamic message generation
- SMTP server communication
- Structured response handling

Basic clean architecture principles were applied to improve maintainability and scalability compared to the initial base code.

## 📌 Author

Angel

---

## 🛠 Technologies Used

- Unity
- C#
- SMTP Protocol
- .NET Mail Libraries

## 📝 License

This project was developed as part of an academic workshop.