# :fire: BANDIT ACS :fire:

# Description

This repository contains the code related to the Access Control Server template, which is meant to be duplicated 4 times for the different banks.
Once parameterized, the template launches an ACS Server which **listens for commands on the specified TCP port**.

## Commands

The server listens for the following commands :

- **ACQCodeValidationCommand** : Used to validate a code sent by an ACQ Server 
- **PaymentFormHandler** : Used to validate user credentials for a payment

# Run the ACS server

## Prerequisites 

- First of all, the ACS server requires the usage of a .pfx file to allow its SSL communications. But be carefull :
  > :warning: **For security reason, the required pfx file is not included in the repository.** You must import it locally inside the /certs directory. This .pfx file is located on the VPS at /etc/pki/tls/tristesse.pfx

- Then, you'll need to install **Docker Desktop** to be able to launch a :whale2: dockerized application :whale2:. 
  > :link: If you don't already have it installed, here is the tutorial : https://docs.docker.com/desktop/install/windows-install/

- Finally, you may need to use **Visual Studio**. Jetbrains Rider should also work but for the moment, the installation steps will only be described for Visual Studio

## Run the code

The ACS server depends on two Databases : **PostgreSQL** and **MongoDB**. Fortunately, the project includes a **docker-compose** configuration to help you setup the proper environement, you just need to launch the project using it and see the :zap: magic :zap: happens! Here's how to use it:

 - 1. Set the "docker-compose" project as the default starting application : 
 
    ![image](https://user-images.githubusercontent.com/91737697/226138025-b31dba3e-dd7c-4689-afa4-dcc4ae7895be.png)

 - 2. Run the code using the green arrow : 

    ![image](https://user-images.githubusercontent.com/91737697/226138242-c6c0da1a-4f9d-4eda-b12b-623203cc6e6a.png)
    
 - 3. You should see 3 containers up and running in Dcoker Desktop:
 
    ![image](https://user-images.githubusercontent.com/91737697/226138519-98756ffa-48e6-4e6a-8426-f1de7606a13c.png)

 - 4. You should now be able to connect to the ACS Server ! By default, the server listens on the port **6000** :muscle: ça va chier marcel :muscle:

## Edit run configuration

You can edit the run configuration by customizing the **docker-compose** file which is located here :

```
/docker/docker-compose.yml
```

### Bandit ACS Parameters 

- **ACS__SQLDatabase__ConnectionString** : Sets the connection string associated to the SQL Database
- **ACS__NOSQLDatabase__ConnectionString** : You're not aussi con que benjamin
- **ACS__SSL__ServerCertificate** : Sets the name of the server certificate (must correspond to local certificate filename)
- **ACS__TCP__Port** : Sets the TCP listening port (docker-compose's port configuration must match this one)
- **Logging__LogLevel__Default** : Sets the log level of the application (use "Information" or "None" for production, "Debug" in Developpement) 

  > :question: For further information about docker-compose configuration, you can check this link : https://docs.docker.com/compose/

# Build a new version

Normally, building a :whale2: dockerized application :whale2: and upload it on a production server requires quite a few steps, but to help you, we've managed to put together a quite simple procedure, thanks to a generation script and an Artifactory !

## Build steps

  1. Once you are satisfied with your code, you can generate a new version of the application by running the :blue_book: **docker-builder.ps1**  :blue_book: script which is located at the rooot of the project. 

      ![image](https://user-images.githubusercontent.com/91737697/226140243-05645bda-194b-4d1a-a3c1-acba9173bbef.png)

  2. The script will automatically fecth the last container version and will suggest you a new appended version number : 

      ![image](https://user-images.githubusercontent.com/91737697/226140141-df828538-998b-43ae-a4d2-da9f68ddbfe0.png)

  3. Simply choose a new version number (or press enter for default), and watch closely, :zap: magic :zap: happens once again !
  
      ![image](https://user-images.githubusercontent.com/91737697/226140292-826a4d15-bb36-474b-9794-c269c3088ead.png)
      
  4. Then, to update the server version used on our VPS, you just need to edit the bandit stack configuration : 

      ![image](https://user-images.githubusercontent.com/91737697/226140416-f22fa081-4295-4441-acce-215dcd3ec20b.png)

Tadaaaam well done !

## Check available versions

To see the available versions for a package, simply go to this url : 
```
https://space.tristesse.lol/p/masi-integratedproject/packages/container/containers/bandit-acs?v=sha256%3Aa8fe0aa99760b0f62e9d64d00879f8472929aa173e3f5c4f1ef83fb7b192bc45&tab=overview
```
  > :warning: The first time you want to access Space, you need to register. For that simply use the following link : https://space.tristesse.lol/oauth/auth/invite/f9a1279cf3b0ffda64cfbd23a7aacf99

hey2
