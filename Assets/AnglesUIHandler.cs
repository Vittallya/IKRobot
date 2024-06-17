using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AnglesUIHandler : MonoBehaviour
{
    private List<TextMeshPro> texts;

    private void Awake()
    {
        texts = this.GetComponentsInChildren<TextMeshPro>().ToList();
    }

    public void OnSolutionsApplyed(List<AxisSolution> solutions)
    {
        for (int i = 0; i < solutions.Count; i++)
        {
            texts[i].text = solutions[i].NewAngle.ToString("0.00");
        }
    }
}
