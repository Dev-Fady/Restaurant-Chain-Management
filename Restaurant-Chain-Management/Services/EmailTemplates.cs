namespace Restaurant_Chain_Management.Services
{
    public static class EmailTemplateService
    {
        public static string GetConfirmEmailTemplate(string name, string confirmationLink)
        {
            return $@"
            <html>
              <head>
                <style>
                  body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    padding: 20px;
                  }}
                  .container {{
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                    max-width: 600px;
                    margin: auto;
                  }}
                  h2 {{
                    color: #333333;
                  }}
                  a {{
                    display: inline-block;
                    padding: 10px 15px;
                    background-color: #4CAF50;
                    color: #ffffff;
                    text-decoration: none;
                    border-radius: 5px;
                    margin-top: 15px;
                  }}
                  p {{
                    color: #555555;
                  }}
                </style>
              </head>
              <body>
                <div class='container'>
                  <h2>Welcome to Restaurant Chain Management 🍽️</h2>
                  <p>Hello {name},</p>
                  <p>Thank you for registering. Please confirm your email address by clicking the button below:</p>
                  <a href='{confirmationLink}'>Confirm Email</a>
                  <p>If you did not request this, please ignore this email.</p>
                  <br/>
                  <p>Best Regards,<br/>Restaurant Chain Team</p>
                </div>
              </body>
            </html>
            ";
        }
    }
}
