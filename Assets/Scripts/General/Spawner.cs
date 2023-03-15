using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //General
    public enum SpawnType { Timed, Replace, Single, Hordes }
    public SpawnType MyType;
    public GameObject Prop, LifeBar;
    public bool SendBar;
    public float Timer;
    public float PlayDist, TrigDist;
    public Transform Player;

    //Timed
    public float Goal;

    //Replace
    public GameObject ToReplace;

    //Single
    public bool SpawnDone;
    public GameObject CurrentProp;

    //Hordes
    [System.Serializable]
    public class HordeProps
    {
        public List<GameObject> Props;
        public List<Vector3> Positions;
    }
    public List<HordeProps> HordesList;
    public List<GameObject> CurrentHorde;
    public int HordeCount;

    //Others
    public List<GameObject> ToDisable, ToEnable;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDone = false;
        HordeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (MyType)
        {
            case SpawnType.Timed:
                //Counts time, and spawns the prop when the moment comes
                Timer += Time.deltaTime;
                if (Timer >= Goal)
                {
                    GameObject NewProp = Instantiate(Prop, transform.position, Quaternion.identity);
                    //if (SendBar == true)
                    //{
                    //    GameObject Bar = Instantiate(LifeBar, transform.position, Quaternion.identity);
                    //    Bar.GetComponent<LifeBarControlOscar>().MyLifeMan = NewProp.GetComponent<LifeNStatusOscar>();
                    //    Bar.GetComponent<LifeBarControlOscar>().Follower = true;
                    //}
                    Timer = 0;
                }
                break;
            case SpawnType.Replace:
                //if the object is not there, waits for a time and then makes it reapear.
                if (ToReplace == null)
                {
                    Timer += Time.deltaTime;
                    if (Timer >= Goal)
                    {
                        ToReplace = Instantiate(Prop, transform.position, Quaternion.identity);
                        //if (SendBar == true)
                        //{
                        //    GameObject Bar = Instantiate(LifeBar, transform.position, Quaternion.identity);
                        //    Bar.GetComponent<LifeBarControlOscar>().MyLifeMan = ToReplace.GetComponent<LifeNStatusOscar>();
                        //    Bar.GetComponent<LifeBarControlOscar>().Follower = true;
                        //}
                        Timer = 0;
                    }
                }
                break;
            case SpawnType.Single:
                //if the player is near, spawns the prop, only once.
                PlayDist = Vector3.Distance(Player.position, transform.position);

                if (SpawnDone == false && PlayDist <= TrigDist)
                {
                    CurrentProp = Instantiate(Prop, transform.position, Quaternion.identity);
                    //if (SendBar == true)
                    //{
                    //    GameObject Bar = Instantiate(LifeBar, transform.position, Quaternion.identity);
                    //    Bar.GetComponent<LifeBarControlOscar>().MyLifeMan = CurrentProp.GetComponent<LifeNStatusOscar>();
                    //    Bar.GetComponent<LifeBarControlOscar>().Follower = true;
                    //}

                    for (int i = 0; i < ToDisable.Count; i++)
                    {
                        ToDisable[i].SetActive(true);
                    }
                    for (int i = 0; i < ToEnable.Count; i++)
                    {
                        ToEnable[i].SetActive(false);
                    }

                    SpawnDone = true;
                }

                if (SpawnDone == true && CurrentProp == null)
                {
                    for (int i = 0; i < ToDisable.Count; i++)
                    {
                        ToDisable[i].SetActive(false);
                    }
                    for (int i = 0; i < ToEnable.Count; i++)
                    {
                        ToEnable[i].SetActive(true);
                    }
                }
                break;
            case SpawnType.Hordes:
                //if the player is near, spawns the first horde, and the next when that one is destroyed, successively
                PlayDist = Vector3.Distance(Player.position, transform.position);

                for (int i = 0; i < CurrentHorde.Count; i++)
                {
                    if (CurrentHorde[i] == null)
                    {
                        CurrentHorde.Remove(CurrentHorde[i]);
                    }
                }

                if (PlayDist <= TrigDist && CurrentHorde.Count == 0)
                {
                    if (HordeCount == 0)
                    {
                        for (int i = 0; i < ToDisable.Count; i++)
                        {
                            ToDisable[i].SetActive(true);
                        }
                        for (int i = 0; i < ToEnable.Count; i++)
                        {
                            ToEnable[i].SetActive(false);
                        }
                    }

                    if (HordeCount < HordesList.Count)
                    {
                        if (HordeCount == 0)
                        {
                            for (int i = 0; i < ToDisable.Count; i++)
                            {
                                ToDisable[i].SetActive(true);
                            }
                            for (int i = 0; i < ToEnable.Count; i++)
                            {
                                ToEnable[i].SetActive(false);
                            }
                        }
                        for (int i = 0; i < HordesList[HordeCount].Props.Count; i++)
                        {
                            GameObject NewProp = Instantiate(HordesList[HordeCount].Props[i], HordesList[HordeCount].Positions[i], Quaternion.identity);
                            CurrentHorde.Add(NewProp);
                            //if (SendBar == true)
                            //{
                            //    GameObject Bar = Instantiate(LifeBar, transform.position, Quaternion.identity);
                            //    Bar.GetComponent<LifeBarControlOscar>().MyLifeMan = NewProp.GetComponent<LifeNStatusOscar>();
                            //    Bar.GetComponent<LifeBarControlOscar>().Follower = true;
                            //}
                        }
                        HordeCount += 1;
                    }
                    else
                    {
                        for (int i = 0; i < ToDisable.Count; i++)
                        {
                            ToDisable[i].SetActive(false);
                        }
                        for (int i = 0; i < ToEnable.Count; i++)
                        {
                            ToEnable[i].SetActive(true);
                        }
                        gameObject.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //ground detector
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TrigDist);
    }
}
