using ImageApp.BLL.Interface;

namespace ImageApp.BLL.Implementation
{
    public class GenerateEmailVerificationPage: IGenerateEmailVerificationPage
	{
        public string EmailVerificationPage(string name, string callbackurl)
        {
            string html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                  <meta charset='UTF-8'>
                  <title>Email Verification</title>
                  <link rel='stylesheet' type='text/css' href='styles.css'>
                    <style>
                        body {{
                          font-family: Arial, sans-serif;
                          background-color: #f5f5f5;
                          margin: 0;
                          padding: 0;
                        }}

                        .container {{
                          max-width: 500px;
                          margin: 100px auto;
                          background-color: #ffffff;
                          border: 1px solid #ccc;
                          padding: 30px;
                          height: auto;
                          border: 1px solid #4CAF50;
                        }}

                        h1 {{
                          font-size: 24px;
                          margin-bottom: 10px;
                        }}

                        p {{
                          font-size: 16px;
                          margin-bottom: 20px;
                          padding: 0px;
                        }}

                        a {{
                            display: inline-block;
                            text-decoration: none;
                            background-color: #4CAF50;
                            color: #fff;
                            padding: 10px 20px;
                            font-size: 14px;
                            position: relative;
                            border: 1px solid #4CAF50;
                            transition: transform 0.2s ease-in;
                            cursor: pointer;
  
                        }}

                        a:hover {{
                           transform: scale(0.95);
                           background-color: #45a049;
                        }}
                    </style>
                </head>
                <body>
                  <div class=""container"">
                     <h1>Email Verification</h1>
                        <p>Hi {name},Thanks for getting started with our Image App!</p>
                        <p>We need a little more information to complete your registration, including a confirmation of your email address.</p>
	                    <p>Click below to confirm your email address:.</p>
                        <div>
                            <a href='{callbackurl}'>Verify</a>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return html;
        }

        public string PasswordResetPage(string callbackurl)
        {
            string html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                  <meta charset='UTF-8'>
                  <title>Email Verification</title>
                  <link rel='stylesheet' type='text/css' href='styles.css'>
                    <style>
                        body {{
                          font-family: Arial, sans-serif;
                          background-color: #f5f5f5;
                          margin: 0;
                          padding: 0;
                        }}

                        .container {{
                          max-width: 500px;
                          margin: 100px auto;
                          background-color: #ffffff;
                          border: 1px solid #ccc;
                          padding: 30px;
                          height: auto;
                          border: 1px solid #4CAF50;
                        }}

                        h1 {{
                          font-size: 24px;
                          margin-bottom: 10px;
                        }}

                        p {{
                          font-size: 16px;
                          margin-bottom: 20px;
                          padding: 0px;
                        }}

                        a {{
                            display: inline-block;
                            text-decoration: none;
                            background-color: #4CAF50;
                            color: #fff;
                            padding: 10px 20px;
                            font-size: 14px;
                            position: relative;
                            border: 1px solid #4CAF50;
                            transition: transform 0.2s ease-in;
                            cursor: pointer;
  
                        }}

                        a:hover {{
                           transform: scale(0.95);
                           background-color: #45a049;
                        }}
                    </style>
                </head>
                <body>
                  <div class=""container"">
                     <h1>Reset Password</h1>
                        <p>We need a little more information to complete your account recovery.</p>
	                    <p>Click below to reset your password.</p>
                        <div>
                            <a href='{callbackurl}'>Change Password</a>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return html;
        }
    }
}
