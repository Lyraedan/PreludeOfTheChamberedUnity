using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DCRichPresence : MonoBehaviour
{

    // Custom presence in discord!

    public Discord.Discord discord;
    public bool debugMode = false;
    [Header("Setup")]
    [Tooltip("Enable or Disable the use of Discord Rich Presence")] public bool useRichPresence = true;
    public long AppClientID = 0;

    [Header("Details")]
    public string topText = string.Empty;
    public string bottomText = string.Empty;

    [Header("Images")]
    public string largeText = string.Empty;
    public string smallText = string.Empty;

    private bool connected = false;
    private StringBuilder builder = new StringBuilder();

    private string largeThumbnailKey = "thumbnail";
    private string smallThumbnailKey = "branding";
    private static long startTime = 0;
    private bool discordRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        if (useRichPresence)
            Connect();
    }

    private void Update()
    {
        if (!discordRunning) return;

        //The user never had discord open when they launched virtubox but then they did launch discord later. If so connect //BUGGY
        /*
        if(!connected && useRichPresence && DiscordIsRunning())
        {
            Connect();
            return;
        }
        */

        try
        {
            if (discord != null && useRichPresence)
            {
                discord.RunCallbacks();
            }
        }
        catch (Exception e)
        {

        }
    }

    void UpdatePresence()
    {
        topText = GetState();
        bottomText = GetKeys(); //$"Elapsed: {GetTimeElapsed()}";
        largeThumbnailKey = GetThumbnail();
        smallThumbnailKey = GetSmallThumbnail();
        UpdateActivity();
    }

    private string GetState()
    {
        return "Exploring " + LocationDisplay.instance.GetLocationName;
    }

    private string GetKeys()
    {
        return "Found " + Player.instance.keys + "/" + Player.instance.maxKeys + " keys!";
    }

    //ref https://discord.com/developers/docs/rich-presence/how-to
    private void UpdateActivity()
    {
        if (discord == null) return;

        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = bottomText,
            Details = topText,
            Timestamps =
            {
                Start = startTime,
            },
            Assets =
            {
                    LargeImage = largeThumbnailKey,
                    LargeText = largeText,
                    SmallImage = smallThumbnailKey,
                    SmallText = smallText
            },
            Instance = true,
        };

        activityManager.UpdateActivity(activity, (result) => {
            if (result == Discord.Result.Ok)
            {
                if (debugMode) Debug.Log("Update Success!");
            }
            else
            {
                if (debugMode) Debug.Log("Update Failed");
            }
        });
    }

    private void ClearActivity()
    {
        if (discord == null) return;

        var activityManager = discord.GetActivityManager();
        activityManager.ClearActivity((result) => {
            if (result == Discord.Result.Ok)
            {
                if (debugMode) Debug.Log("Clear Success!");
            }
            else
            {
                if (debugMode) Debug.Log("Clear Failed");
            }
        });
    }

    public void RefreshActivity()
    {
        ClearActivity();
        UpdateActivity();
    }

    private void OnLevelWasLoaded(int level)
    {
        UpdatePresence();
    }

    string GetThumbnail()
    {
        var location = LocationDisplay.instance.GetLocationName.ToLower();
        if (location.Contains("prison"))
            return "prison";
        else if (location.Contains("dungeon"))
            return "dungeon";
        else if (location.Contains("overworld"))
            return "overworld";
        return "default";
    }

    string GetSmallThumbnail()
    {
        smallText = "";
        return ""; // default
    }

    private void Connect()
    {
        discordRunning = DiscordIsRunning();
        if (!discordRunning) return;

        System.Environment.SetEnvironmentVariable("DISCORD_INSTANCE_ID", "0");
        //System.Environment.SetEnvironmentVariable("DISCORD_INSTANCE_ID", "1");
        discord = new Discord.Discord(AppClientID, (System.UInt64)Discord.CreateFlags.Default);
        Discord.UserManager userManager = discord.GetUserManager();
        userManager.OnCurrentUserUpdate += () => {
            var currentUser = userManager.GetCurrentUser();
            if (!string.IsNullOrEmpty(currentUser.Username))
            {
                if (debugMode) Debug.Log("Logged into discord as " + currentUser.Username);
                connected = true;
                var activityManager = discord.GetActivityManager();
                //startTime = Convert.ToInt64(Time.time);
                DateTime now = DateTime.Now;
                long unixTime = ((DateTimeOffset)now).ToUnixTimeSeconds();
                startTime = unixTime;
                if (debugMode) Debug.Log("Updating presence");

                topText = GetState();
                bottomText = GetKeys();
                largeThumbnailKey = GetThumbnail();
                smallThumbnailKey = GetSmallThumbnail();

                RefreshActivity();
                UpdatePresence();
                InvokeRepeating("UpdateTimer", 1f, 1f); //Updates time
            }
        };
    }

    void UpdateTimer()
    {
        UpdatePresence();
    }

    private void Disconnect()
    {
        ClearActivity();
        discord.Dispose();
    }

    private void OnApplicationQuit()
    {
        if (connected && useRichPresence)
            Disconnect();
    }

    bool DiscordIsRunning()
    {
        System.Diagnostics.Process[] pname = System.Diagnostics.Process.GetProcessesByName("Discord");
        if (pname.Length > 0)
        {
            if (debugMode) Debug.Log("Discord is running!");
            return true;
        }
        else
        {
            if (debugMode) Debug.LogError("Discord is not running!");
            return false;
        }
    }

}
