using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class AddressManager : MonoBehaviour
{
    public Transform container;
    public GameObject contactsview;
    public GameObject contactPrefab;

    public static AddressManager Instance;
    AddressBookContactsAccessStatus status;
    public IAddressBookContact[] allContacts; //IG_AddressBookService.instance.allContacts[index];

    public bool isLoading = true;
    public TMP_Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CheckAndRequestContactPermissions();
    }
    //IG_AddressBookService.instance.ReadContacts();

    private void CheckAndRequestContactPermissions()
    {
        // Check if the user has authorized contact permissions
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.READ_CONTACTS"))
        {
            // If not authorized, request permission
            UnityEngine.Android.Permission.RequestUserPermission("android.permission.READ_CONTACTS");
            ReadContacts();
        }
        else
        {
            // If already authorized, proceed to read contacts
            ReadContacts();
        }
    }

    public void ReadContacts()
    {
        status = AddressBook.GetContactsAccessStatus();
        if (status == AddressBookContactsAccessStatus.NotDetermined)
        {
            AddressBook.RequestContactsAccess(callback: OnRequestContactsAccessFinish);
        }
        if (status == AddressBookContactsAccessStatus.Authorized)
        {
            AddressBook.ReadContacts(OnReadContactsFinish);
        }
    }
    private void OnRequestContactsAccessFinish(AddressBookRequestContactsAccessResult result, Error error)
    {
        Debug.Log("Request for contacts access finished.");
        Debug.Log("Address book contacts access status: " + result.AccessStatus);
        if (result.AccessStatus == AddressBookContactsAccessStatus.Authorized)
        {
            AddressBook.ReadContacts(OnReadContactsFinish);
        }
    }
    private void OnReadContactsFinish(AddressBookReadContactsResult result, Error error)
    {
        if (error == null)
        {
            allContacts = result.Contacts;
            var contacts = result.Contacts;
            Debug.Log("Request to read contacts finished successfully.");
            Debug.Log("Total contacts fetched: " + contacts.Length);
            Debug.Log("Below are the contact details (capped to first 10 results only):");
            isLoading = false;
            loadingText.gameObject.SetActive(false);

            for (int iter = 0; iter < contacts.Length; iter++)
            {
                Debug.Log(string.Format("[{0}]: {1}", iter, contacts[iter]));
                CreateUserPrefab(contacts[iter]);
            }
        }
        else
        {
            Debug.Log("Request to read contacts failed with error. Error: " + error);
        }
    }

    

    public void CreateUserPrefab(IAddressBookContact contact)
    {
        GameObject go = Instantiate(contactPrefab, container);
        go.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = contact.FirstName;
        go.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = contact.LastName;
        if (contact.EmailAddresses != null && contact.EmailAddresses.Length > 0)
        {
            UnityEngine.UI.Button button = go.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>();
            button.interactable = true; // Enable the button
            button.onClick.AddListener(() => OnContactButtonClick(contact.EmailAddresses[0]));
        }
        else
        {
            // If no email address, disable or hide the button
            go.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void OnContactButtonClick(string mailId)
    {
        ShareMail.instance.ShareMailText(mailId);
    }



    
}
