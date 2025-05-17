using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class RouteRender : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private int lastDestiny=0;
    public List<Vector3> plotRoute(int destiny)//�����귵����Щ������
    {
        List<Vector3> points = RandomPoints(destiny, GameConfig.RoutePointNum);
        Vector3 camp= points[0];
        points=points.OrderBy(p => Vector3.Distance(camp,p)).ToList();
        plotline(points);
        return points;
    }
    private void plotline(List<Vector3>points)
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    private List<Vector3> RandomPoints(int destiny,int num)//����Ӫ�ص�Ŀ�ĵصĽڵ㣬����Ӫ�غ�Ŀ�ĵأ���һ��Ԫ��Ϊ������ destiny:Ŀ��ر��,0ΪӪ�� 1-4���Ͽ�ʼ˳ʱ��,num���ɽ�����
    {
        List<Vector3>Ends= GetEnds(destiny);
        List<Vector3> res = new List<Vector3>();
        foreach (Vector3 end in Ends)
        {
            res.Add(end);
        }

        float sx = Ends[0].x;
        float sy = Ends[0].y;
        float ex = Ends[1].x;
        float ey = Ends[1].y;
        System.Random r = new System.Random();
        for(int i = 0; i < num; i++)
        {
            float min = sx < ex ? sx : ex;
            float x = (float)r.NextDouble() * math.abs(ex - sx) + min;
            min = sy < ey ? sy : ey;
            float y = (float)r.NextDouble() * math.abs(ey - sy) + min;
            Vector3 point= new Vector3(x,y,0);
            bool near = false;//����������ɾ��
            foreach(Vector3 p in res)
            {
                if (Vector3.Distance(p, point)<GameConfig.PointDistance)
                {
                    near = true;
                    break;
                }
            }
            if (near)
            {
                i--;
                continue;
            }
            res.Add(point);
        }
        return res;
    }
    private List<Vector3>GetEnds(int destiny)//����Ŀ�ĵػ�ȡ�߽�� destiny:Ŀ��ر��,0ΪӪ�� 1-4���Ͽ�ʼ˳ʱ��
    {
        List<Vector3>Ends=new List<Vector3>();
        Vector3 point,_point = new Vector3();
        Vector2 size = new Vector2();
        GameObject D;
        switch (destiny)
        {
            case 0:
                Ends.Add(transform.Find("Player").position);
                point = transform.Find("Camp").gameObject.GetComponent<Camp>().EndPoint(lastDestiny);
                Ends.Add(point);
                break;
            case 1:
                point=transform.Find("Camp").gameObject.GetComponent<Camp>().EndPoint(destiny);
                Ends.Add(point);
                D= transform.Find(GameConfig.Destinies[destiny]).gameObject;
                size=D.GetComponent<SpriteRenderer>().bounds.size;
                _point = D.transform.position + new Vector3(size.x/2,-size.y/2,0);
                Ends.Add(_point);
                lastDestiny = destiny;
                break;
            case 2:
                point = transform.Find("Camp").gameObject.GetComponent<Camp>().EndPoint(destiny);
                Ends.Add(point);
                D = transform.Find(GameConfig.Destinies[destiny]).gameObject;
                size = D.GetComponent<SpriteRenderer>().bounds.size;
                _point = D.transform.position + new Vector3(-size.x / 2, -size.y / 2, 0);
                Ends.Add(_point);
                lastDestiny = destiny;
                break;
            case 3:
                point = transform.Find("Camp").gameObject.GetComponent<Camp>().EndPoint(destiny);
                Ends.Add(point);
                D = transform.Find(GameConfig.Destinies[destiny]).gameObject;
                size = D.GetComponent<SpriteRenderer>().bounds.size;
                _point = D.transform.position + new Vector3(-size.x / 2, size.y / 2, 0);
                Ends.Add(_point);
                lastDestiny = destiny;
                break;
        }
        return Ends;
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;  // �����
        lineRenderer.endWidth = 0.1f;    // �յ���
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
