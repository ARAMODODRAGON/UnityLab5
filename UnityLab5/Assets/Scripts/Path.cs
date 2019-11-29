using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

	[SerializeField]
	private bool drawGizmos = false;

	[SerializeField]
	private Transform[] points;

	public Vector2 GetPoint(int index) => points[index].position;
	public int PointCount => points.Length;

	/// Debug drawing
	private void OnDrawGizmos() {
		if (!drawGizmos) return;

		// only draw points if there are at least 2 points
		if (points == null || points.Length < 2) return;

		// set colors
		Color lineColor = Color.Lerp(Color.red, Color.white, 0.4f);
		Color pointColor = Color.Lerp(Color.blue, Color.white, 0.4f);

		int i = 0;
		for (; i < points.Length - 1; i++) {
			// this prevents it from trying to draw a path that doesnt have a transform on one of the points
			if (points[i] == null || points[i + 1] == null) break;

			// draw a line
			Gizmos.color = lineColor;
			Gizmos.DrawLine(points[i].position, points[i + 1].position);
			// draw a point
			Gizmos.color = pointColor;
			Gizmos.DrawWireSphere(points[i].position, 0.5f);
		}
		// draw the last point
		if (i < points.Length && points[i] != null)
			Gizmos.DrawWireSphere(points[i].position, 0.5f);
	}
}
