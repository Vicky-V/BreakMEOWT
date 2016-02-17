using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour 
{
    Camera m_Camera;
    float m_DefalutSize;

    public Vector3 DefaultPosition
    {
        get { return m_DefaultPosition; }
        private set { m_DefaultPosition = value; }
    }
    Vector3 m_DefaultPosition;

    Coroutine m_ZoomCR;
    Coroutine m_PanCR;

    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_DefalutSize = m_Camera.orthographicSize;
        DefaultPosition = transform.position;
        m_ZoomCR = null;
        m_PanCR = null;
    }

    public void Pan(Vector3 direction, float panTime)
    {
        if (m_PanCR != null)
            StopCoroutine(m_PanCR);

        m_PanCR = StartCoroutine(pan_cr(direction, panTime));
    }

    IEnumerator pan_cr(Vector3 direction, float time)
    {
        float t = 0;
        Vector3 startPos = transform.position;

        while (true)
        {
            float currentTime = t / time;
            Vector3 posOffset = Vector3.Lerp(startPos, startPos + direction, currentTime);

            transform.position = posOffset;

            t += Time.deltaTime;

            if (t > time)
                t = time;

            yield return null;
        }

        yield return null;
    }

    public void Zoom(float zoomScale, float zoomTime)
    {
        if(m_ZoomCR!=null)
            StopCoroutine(m_ZoomCR);

        m_ZoomCR = StartCoroutine(zoom_cr(zoomScale, zoomTime));
    }

    IEnumerator zoom_cr(float endScale, float time)
    {
        float t = 0;
        float startSize = m_Camera.orthographicSize;

        while(true)
        {
            float currentTime = t / time;
            float zoomScale = Mathf.Lerp(startSize/m_DefalutSize, endScale, currentTime);
            
            m_Camera.orthographicSize = m_DefalutSize * zoomScale;
           
            t += Time.deltaTime;
            
            if (t > time)
                t = time;

            yield return null;
        }

        yield return null;
    }
}
