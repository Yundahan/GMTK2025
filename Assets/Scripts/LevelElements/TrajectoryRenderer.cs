using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    public GameObject trajectoryLine;

    private void Awake()
    {
        this.trajectoryLine.transform.localScale = new Vector3(1f, 0.1f, 1f);
    }

    void Update()
    {
        if (GetComponent<Key>().IsInPlayerHand())
        {
            this.trajectoryLine.SetActive(true);
            Vector2 trajectory = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
            float xAngle = Mathf.Atan2(trajectory.y, trajectory.x) * 180 / Mathf.PI;
            this.trajectoryLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, xAngle));
            this.trajectoryLine.transform.localScale = new Vector3(trajectory.magnitude * 1.5f, 0.1f, 1f);
            this.trajectoryLine.transform.localPosition = trajectory * 0.75f;
        } else
        {
            this.trajectoryLine.SetActive(false);
        }
    }

    public void Reset()
    {
        this.trajectoryLine.SetActive(false);
    }
}
