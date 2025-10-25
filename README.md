# What is SmtpTelegramGateway

SmtpTelegramGateway is an SMTP gateway that forwards received emails to specified Telegram chats via your Telegram bot. Runs as a windows service, as a unix daemon or as a standalone application. Fully written in C#.

# Setup

1. Edit `appsettings.yaml`. At least specify a telegram bot token and a chat id.
    ```yaml
    # The port that the gateway will listen on to receive SMTP e-mail messages, the default is 25. 
    # No authorization is required when connecting to this port, select Basic Authorizathion if it is required
    SmtpPort: 25
    # Your token for the Telegram bot, get it at https://t.me/BotFather when registering the bot
    TelegramBotToken: SPECIFY THERE TELEGRAM BOT TOKEN
    # Define here a list of email addresses and Telegram chats that will receive emails sent to these addresses.
    # Use an asterisk "*" instead of an email address to send all emails to some Telegram chat
    # If you specify a Telegram user chat, the user must be subscribed to the bot
    # If you specify a Telegram group chat, you may need to add a minus sign prior to the group id, the bot must be added to the group
    # If you specify a Telegram channel chat, you may need to add -100 prior to the channel id, the bot must be added to the channel admins and given the right "Post in the channel"
    # For public channel chat, you can specify the channel public @username instead of the channel id
    Routing:
      - Email: "*"
        TelegramChat: SPECIFY THERE TELEGRAM USERID, GROUPID, CHANNELID OR @USERNAME
      - Email: example@test.com
        TelegramChat: SPECIFY THERE TELEGRAM USERID, GROUPID, CHANNELID OR @USERNAME
    # Logging Level. Set to Debug to see the details of the communication between your mail program and the gateway.
    # Set to Error to see less information
    Logging:
      LogLevel:
        Default: Debug
    ```
2. Register and run
    - Run `SmtpTelegramGateway.exe` as a standalone application
    - Or register the program as a windows service
        ```ps
        sc.exe create "SMTP Telegram Gateway" binpath="C:\Program Files\SmtpTelegramGateway\SmtpTelegramGateway.exe" start=auto obj="NT AUTHORITY\LocalService"
        ```
        then start the windows service
        ```ps
        sc.exe start "SMTP Telegram Gateway"
        ```
    - Or register the program as a systemd service in unix-like operating systems. Create a configuration file `/etc/systemd/system/smtp-telegram-gateway.service` looking as follows:
        ```ini
        [Unit]
        Description=SMTP Telegram Gateway
        [Service]
        Type=simple
        ExecStart=/usr/sbin/SmtpTelegramGateway
        [Install]
        WantedBy=multi-user.target
        ```
        then say systemd to load the new configuration file
        ```console
        sudo systemctl daemon-reload
        ```
        and run the service
        ```console
        sudo systemctl start smtp-telegram-gateway.service
        ```

4. Send a test email and get it in Telegram. Use `localhost` as an SMTP server address, `25` as a port and no authentifiacation or, if necessary, select the basic authentication method with a fake username and password.

# Other configure options

Apart from configuration via "appsettings.yaml," the program can also be configured using command line parameters:
   ```ps
   SmtpTelegramGateway.exe --SmtpPort 25 --TelegramBotToken 111:ABC --Routing:0:Email * --Routing:0:TelegramChat 222 --Logging:LogLevel:Default Debug
   ```
or using environment variables:
   ```
   SmtpPort=25
   TelegramBotToken=111__ABC
   Routing__0__Email=*
   Routing__0__TelegramChat=222
   Logging__LogLevel__Default=Debug
   ```
or using file `appsettings.json`:
   ```json
   {
     "SmtpPort": 25,
     "TelegramBotToken": "SPECIFY THERE TELEGRAM BOT TOKEN",
     "Routing": [
       {
         "Email": "*",
         "TelegramChat": "SPECIFY THERE TELEGRAM USERID, GROUPID, CHANNELID OR @USERNAME"
       },
       {
         "Email": "example@test.com",
         "TelegramChat": "SPECIFY THERE TELEGRAM USERID, GROUPID, CHANNELID OR @USERNAME"
       }
     ],
     "Logging": {
       "LogLevel": {
         "Default": "Debug"
       }
     }
   }
   ```
You can also combine these methods.