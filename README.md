# WSW

## Email
WSW currently supports sending mail notification through following accounts:
*	GMAIL
*	OUTLOOK

Update following attributes of **email** node in **mail.config.xml**

| Attribute             | Meaning                                  |
|-----------------------|------------------------------------------|
| fromMailAddress       | Mail sender address          			   |
| fromDisplayName       | Display name for sending email address   |
| toMailAddress         | Mail receiving comma-separated addresses |
| enableSsl             | Enable SSL or not                        |
| timeoutInMilliseconds | Timeout for sending email                |

## Password

In **App.config**. 
Replace **YOUR_PASSWORD** with plain password.
```
<!--<secure fromMailPassword="YOUR_PASSWORD"/>-->
```
After first run, WSW.exe.config will contain the encrypted **secure** section. No need for plain password after that.


## Services
Service settings can be configured in **serviceSettings.config.xml**

**serviceCategory:** We can categories services using the category attribute.

**services:** Collection of **service**.

**service:** We can have multiple service elements in services collection.

| Attribute          | Meaning                                                                                                                                                                                                                                                                                                                               |
|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| name               | Name of service                                                                                                                                                                                                                                                                                                                       |
| Source             | Value of the **Source** column of a log entry in Event Viewer                                                                                                                                                                                                                                                                           |
| logName            | Value of the **Log** column of a log entry in Event Viewer                                                                                                                                                                                                                                                                              |
| enableNotification | Enable/disable email notification for a service without restarting WSW.                                                                                                                                                                                                                                                               |
| enableStart        | Enable/disable restart for a service without restarting WSW.                                                                                                                                                                                                                                                                          |
| isMailSent         | If unfortunately WSW crashes or someone stops it, then after it gets restarted it needs to know whether email notification for currently stopped configured services has already been sent before WSW failure or not. This attribute helps WSW in deciding that. **DON’T CHANGE ITS VALUE MANUALLY. LET IT BE FALSE FOR THE FIRST TIME.** |
| waitTimeoutForStartInSeconds| Timeout value for restarting a windows service																																																																					 |
| otherConfigs       | Comma-separated relative paths of files/folders for other configuration files whose details needs to be part of the email notification                                                                                                                                                                                                |
