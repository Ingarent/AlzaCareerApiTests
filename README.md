# AlzaCareerApiTests

Automated test suite for verifying the correctness of job listing data from Alza.cz's public career API.  
Created in C# using .NET 8, Selenium WebDriver, NUnit, and log4net.

---

## 📌 Project Goal

As QA engineers, we are responsible for ensuring that each job posting contains all required information.  
To reduce manual effort, we automate this verification via UI-based API parsing using Selenium.

---

## ✅ What Is Tested

Using endpoint:  
`https://webapi.alza.cz/api/career/v2/positions/java-developer-`

### 📝 Job Description
- The job **must contain** a description (`description`)
- The job **must be marked** as suitable for students (`forStudents == true`)

### 🏢 Place of Employment
- Name: `"Hall office park"`
- Country: `"Česká republika"`
- City: `"Praha"`
- Street: `"U Pergamenky 2"`
- Postal code: `"17000"`

### 👤 Executive User (Nadřízený)
- Executive **must exist**
- Name: `"Kozák Michal"`
- Must have a **photo**
- Must have a **description**

### ❌ Invalid Endpoint Check
- Invalid job URLs return appropriate `404` or error message in HTML

---

## ⚙️ Technologies Used

- [.NET 8](https://dotnet.microsoft.com/en-us/download)
- [NUnit](https://nunit.org/)
- [Selenium WebDriver](https://www.selenium.dev/)
- [log4net](https://logging.apache.org/log4net/)
- (Planned: [RestSharp](https://restsharp.dev/), [docfx](https://dotnet.github.io/docfx/))

---

## 🧪 How to Run

You can run tests via Visual Studio Test Explorer or via CLI:

```bash
dotnet test
