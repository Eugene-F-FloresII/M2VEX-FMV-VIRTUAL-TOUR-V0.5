using System.Collections;
using TMPro;
using UnityEngine;

public class RhythmGame : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] AudioSource musicSource;
    AudioSource OtherSource;   // assign your song here in Inspector
    public float bpm = 120f;          // beats per minute of your song

    [Header("Note Settings")]
    public GameObject notePrefab;     // assign a cube or prefab in Inspector
    public Transform spawnPoint;
    public GameObject results;
    public GameObject currentstatus;
    public AudioClip[] audioClip;
   

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreHit;
    public TextMeshProUGUI MissedNotes;
    public TextMeshProUGUI Overallscore;
    public TextMeshProUGUI status;// where notes spawn

    private float beatInterval;       // seconds per beat
    private float nextBeatTime;       // when the next beat happens
    private float songStartTime;

    private int spawnedNotes;
    private int currentscore;
    private int missednotes;

    //PadlockStatus padlockstatus;

    private bool resultsShown = false;


    private void OnEnable()
    {
        OtherSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
       // padlockstatus = GameObject.FindGameObjectWithTag("PadlockHolder").GetComponent<PadlockStatus>();
        OtherSource.gameObject.SetActive(false);
    }

    void Start()
    {
        currentstatus.SetActive(false);
        results.SetActive(false);
        if (musicSource == null)
        {
            Debug.LogError("No AudioSource assigned!");
            return;
        }

        beatInterval = 60f / bpm;
        songStartTime = Time.time;

        musicSource.clip = audioClip[0];
        musicSource.Play(); // start music
        nextBeatTime = songStartTime + beatInterval; // schedule first beat


    }

    void Update()
    {
        float songTime = Time.time - songStartTime;

        if (songTime >= nextBeatTime - songStartTime)
        {
            if (spawnedNotes < 30)
            {
                SpawnNote();
                nextBeatTime += beatInterval;
            }
            else if (!resultsShown)
            {
                musicSource.Stop();
                Results();
                resultsShown = true;
            }
        }
    }

    void SpawnNote()
    {
        if (notePrefab != null && spawnPoint != null)
        {
            Instantiate(notePrefab, spawnPoint);
            spawnedNotes++;
        }
    }

    public void playerScore(int points)
    {
        scoreText.text = ("Score: " + (currentscore + points));
        currentscore++;
    }

    public void missedScore()
    {
        missednotes++;
    }

    public void Results()
    {
        results.SetActive(true);
        int overall = currentscore - missednotes;
        ScoreHit.text = ("" + currentscore);
        MissedNotes.text = ("" + missednotes);
        Overallscore.text = ("" + (overall));

        StartCoroutine(SetStatus(overall));

    }

    IEnumerator SetStatus(int stats)
    {
        currentstatus.SetActive(true);
        yield return new WaitForSeconds(1f);

        if (stats > (spawnedNotes * 0.5))
        {
            status.text = ("<color=#008000>Goal Reached!</color>");
            StartCoroutine(EndGame());
            //SatisfactoryBar.instance.AddSatisfaction(25);
            //EventLogs.instance.IncreaseSatisfaction(25);
            //GameManager.Instance.CheckGames();
            OtherSource.gameObject.SetActive(true);

        } else if (stats < (spawnedNotes * 0.5))

        {
            status.text = ("<color=#ff0000>Goal Failed!</color>");
            StartCoroutine(EndGame());
            //SatisfactoryBar.instance.TakeSatisfaction(25);
            //EventLogs.instance.DecreaseSatisfaction(25);
            //GameManager.Instance.CheckGames();
            OtherSource.gameObject.SetActive(true);

        } else if (stats == (spawnedNotes * 0.5))
        {
            status.text = ("Goal Barely Reached");
            StartCoroutine(EndGame());
           // SatisfactoryBar.instance.TakeSatisfaction(0);
            //GameManager.Instance.CheckGames();
            OtherSource.gameObject.SetActive(true);
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        resultsShown = false;
        Destroy(gameObject);
       // GlobalManager.instance.Padlock(padlockstatus.name, false);
    }
}
