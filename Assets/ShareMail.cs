using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;


public class ShareMail : MonoBehaviour
{

    public static ShareMail instance;

    private void Awake()
    {
        instance = this;
    }
    public void ShareMailText(string mailId)
    {
        bool canSendMail = MailComposer.CanSendMail();
        if(canSendMail)
        {
            MailComposer composer = MailComposer.CreateInstance();
            composer.SetToRecipients(new string[1] { mailId });

            composer.SetSubject("Beat my High Score");
            composer.SetBody("Body", false);//Pass true if string is html content
            composer.SetCompletionCallback((result, error) => {
                Debug.Log("Mail composer was closed. Result code: " + result.ResultCode);
            });
            composer.Show();
        }
    }
}
