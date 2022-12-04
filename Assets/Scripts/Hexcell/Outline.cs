/*
//  Copyright (c) 2015 José Guerreiro. All rights reserved.
//
//  MIT license, see http://www.opensource.org/licenses/mit-license.php
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

	//[RequireComponent(typeof(Renderer))]
	/* [ExecuteInEditMode] */
public class Outline : MonoBehaviour
{
	public GridInventory inventory;
	private bool last_enabled = false;
	private int last_color = 0;
	public bool enabled = false;
	public int color = 0;
	public Material[] material = new Material[2];
	void Start()
	{

	}
	void Update()
	{
        if (last_color != color || last_enabled != enabled)
        {
			last_color = color;
			last_enabled = enabled;
            if (enabled)
            {
				MeshRenderer cellRenderer = GetComponent<MeshRenderer>();
				cellRenderer.sharedMaterial = material[color];
			}
            else
            {
				MeshRenderer cellRenderer = GetComponent<MeshRenderer>();
				int Landform = GetComponent<Detail>().Landform;
				cellRenderer.sharedMaterial = Landform switch
				{
					3 => inventory.mountain3.material,
					2 => inventory.hill2.material,
					1 => inventory.hill1.material,
					0 => inventory.ground0.material,
					-1 => inventory.pit_1.material,
					-2 => inventory.basin_2.material,
					-3 => inventory.valley_3.material,
					_ => inventory.hole.material,
				};
			}
        }
	}
	public void LandformChange()
	{
		if (enabled)
		{
			MeshRenderer cellRenderer = GetComponent<MeshRenderer>();
			cellRenderer.sharedMaterial = material[color];
		}
		else
		{
			MeshRenderer cellRenderer = GetComponent<MeshRenderer>();
			int Landform = GetComponent<Detail>().Landform;
			cellRenderer.sharedMaterial = Landform switch
			{
				3 => inventory.mountain3.material,
				2 => inventory.hill2.material,
				1 => inventory.hill1.material,
				0 => inventory.ground0.material,
				-1 => inventory.pit_1.material,
				-2 => inventory.basin_2.material,
				-3 => inventory.valley_3.material,
				_ => inventory.hole.material,
			};
		}
	}
}
