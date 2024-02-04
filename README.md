# Unity Contacts Access and Mail Sharing Feature - Setup Guide

This document provides a step-by-step guide for implementing a Unity project with contacts access and mail sharing features using the Essential Kit assets.

## Prerequisites
- Unity installed on your machine.
- Essential Kit asset is added to your Unity project.

## Steps

### 1. Install Essential Kit and Setup Address Book
- Import Essential Kit into your Unity project.
- Set up the Address Book feature in Essential Kit.

### 2. Create UI for Contact List
- Design the UI for displaying the contact list. This UI should include a prefab for individual contacts.

### 3. Write Android Manifest Permissions
- In the `AndroidManifest.xml` file, ensure that the necessary permissions are added for accessing contacts. Add the following lines:

<uses-permission android:name="android.permission.READ_CONTACTS" />

### 4. Attach Address Manager Script
- Attach the AddressManager script to a GameObject in scene. This script handles the logic for reading contacts and instantiating the contact prefab.
- In the AddressManager script, implement the logic to read contacts using Essential Kit's Address Book feature.
- Instantiate the contact prefab in the scene view for each contact.
- Check if an email address is present for each contact.
- If an email address is present, enable the "Send Mail" button for that contact.

### 5. Add ShareMail Script
- Add the ShareMail script to handle the functionality of sharing emails.

