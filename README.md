# WSW: How to change configuration settings?

## Email
WSW currently supports sending mail notification through following accounts:
*	GMAIL
*	OUTLOOK

**File:** App.config

**Configurable:** **configSource** attribute of **email** node.

**Example:**

```
<email configSource="sections\email\gmail.config.xml"/>
```

Update following attributes of **email** node in **gmail.config.xml** / **outlook.config.xml**

| Attribute             | Meaning                                  |
|-----------------------|------------------------------------------|
| fromMailAddress       | Email address which sends email          |
| fromDisplayName       | Display name for sending email address   |
| toMailAddress         | Email Address which receives email       |
| toDisplayName         | Display name for receiving email address |
| enableSsl             | Enable SSL or not                        |
| timeoutInMilliseconds | Timeout for sending email                |

## Password

**File:** App.config

**Configurable:** **configSource** attribute of **secure** node.

**Example:**

```
<secure configSource="sections\secure\gmail.pass.config.xml"/>
```

Update following attributes of **email** node in **gmail.pass.config.xml** / **outlook.pass.config.xml**

| Node        | Meaning                                                                                                           |
|-------------|-------------------------------------------------------------------------------------------------------------------|
| CipherValue | Text value in this node is the encrypted password. Replace **YOUR_ENCRYPTED_PASSWORD** placeholder with your password |

Note: Above steps are for encrypted password, but if you want to use plain password, then comment the line 
```
<secure configSource="sections\secure\gmail.pass.config.xml"/>
```

and uncomment the below line in **App.config**. 
Update your password in **fromMailPassword** attribute.

```
<!--<secure fromMailPassword="YOUR_PASSWORD"/>-->
```

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
