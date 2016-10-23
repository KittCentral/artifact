using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace Galaxy
{
	enum byteNames {Name, Comp, DistRel, RAh, RAm, RAs, DE, DEd, DEm, pm, u_pm, pmPA, RV, n_RV, Sp, r_Sp, Vmag, r_Vmag, n_Vmag, BV, r_BV, n_BV, UB, r_UB, n_UB, RI, r_RI, n_RI, trplx, e_trplx, 
		plx, e_plx, n_plx, Mv, n_Mv, q_Mv, U, V, W, HD, DM, Giclas, LHS, OtherName, Remarks};

	public class GalaxyControl : MonoBehaviour 
	{
		public float zoom;
        string name;
        string specType;
        string otherName;
        string remarks;
        double dec, asc, par;
		public GameObject star;
		GameObject clone;
		char enter = (char)10;
		double size;
        float mag;
		Color colorMat;

		int[] bytePos = {1, 9, 11, 13, 16, 19, 22, 23, 26,31,37,38,44,51,55,67,68,74,75,76,81,82,83,88,89,90,95,96,97,103,109,115,120,122,127,129,132,137,142,147,154,167,177,183,189,257};

		void Start ()
		{
			RetrieveData();
		}

		void RetrieveData()
		{
			using (BinaryReader reader = new BinaryReader(File.Open("GLIESE3.DAT", FileMode.Open)))
			{


				long lineStart = 1;
				for(int starNum = 0; starNum < 3803; starNum++)
				{
					for(int index = 0; index < 45; index++)
					{
						//int index = (int)byteNames.Vmag;
						bool doubleBreak = false;
						byte[] buffer = new byte[bytePos[index+1]-bytePos[index]];
						reader.BaseStream.Position = bytePos[index]+lineStart-1;
						for(int i = 0; i < buffer.Length; i++)
						{
							buffer[i] = reader.ReadByte();
							if(enter == Convert.ToChar(buffer[i]))
							{
								lineStart = reader.BaseStream.Position;
								doubleBreak = true;
								break;
							}
						}
						string result = System.Text.Encoding.UTF8.GetString(buffer);
						switch (index)
						{
						    case (int)byteNames.Name:
							    result = Regex.Replace(result, @"\s+", "");
                                if (result == "NN")
                                    result = "No Name";
							    name = result;
                                break;
						    case (int)byteNames.RAh:
							    result = Regex.Replace(result, @"\s+", "");
							    try
							    {
								    asc = Convert.ToDouble(result);
							    }
							    catch
							    {
								    asc = 0;
							    }
							    break;
						    case (int)byteNames.RAm:
							    result = Regex.Replace(result, @"\s+", "");
							    try
							    {
								    asc += Convert.ToDouble(result)/60;
							    }
							    catch
							    {
								    asc = asc;
							    }
							    break;
						    case (int)byteNames.RAs:
							    result = Regex.Replace(result, @"\s+", "");
							    try
							    {
								    asc += Convert.ToDouble(result)/3600;
							    }
							    catch
							    {
								    asc = asc;
							    }
							    break;
						    case (int)byteNames.DEd:
							    result = Regex.Replace(result, @"\s+", "");
							    if(result == "")
								    result = "0";
							    dec *= Convert.ToDouble(result);
							    break;
						    case (int)byteNames.DE:
							    result = Regex.Replace(result, @"\s+", "");
							    if(result == "-")
								    dec = -1;
							    else
								    dec = 1;
							    break;
						    case (int)byteNames.Sp:
							    result = Regex.Replace(result, @"\s+", "");
                                specType = result;
                                if (result != "")
							    {
								    if (result[0] == 'O' || result[0] == 'o')
									    colorMat = new Color(50, 50, 255);
								    else if (result[0] == 'B' || result[0] == 'b')
									    colorMat = new Color(100, 100, 255);
								    else if (result[0] == 'A' || result[0] == 'a')
									    colorMat = new Color(150, 150, 255);
								    else if (result[0] == 'G' || result[0] == 'g')
									    colorMat = new Color(255, 200, 150);
								    else if (result[0] == 'K' || result[0] == 'k')
									    colorMat = new Color(255, 150, 50);
								    else if (result[0] == 'M' || result[0] == 'M')
									    colorMat = new Color(255, 100, 100);
								    else
									    colorMat = new Color(255, 255, 255);
							    }
							    else
							    {
								    colorMat = Color.white;
							    }
							    break;
						    case (int)byteNames.Mv:
							    result = Regex.Replace(result, @"\s+", "");
							    if(result == "")
								    result = "0";
							    size = Convert.ToDouble(result);
                                size = size >= .1f ? size : .1f;
							    break;
						    case (int)byteNames.plx:
							    result = Regex.Replace(result, @"\s+", "");
							    if(result == "")
								    result = "0";
							    par = Convert.ToDouble(result);
							    break;
                            case (int)byteNames.OtherName:
                                result = Regex.Replace(result, @"\s+", "");
                                otherName = result;
                                if (otherName != "" && name == "No Name")
                                    name = "";
                                break;
                            case (int)byteNames.Remarks:
                                result = Regex.Replace(result, @"\s+", "");
                                remarks = result;
                                break;
                            default:
							    break;
						}
						if(doubleBreak)
							break;
					}
					Location loc = new Location((float)dec,(float)asc,(float)par);
                    if (new Vector3(Mathf.Ceil(loc.Coord.x * 10), Mathf.Ceil(loc.Coord.y * 10), Mathf.Ceil(loc.Coord.z * 10)) != new Vector3(-13, -43, 112))
                    {
                        clone = Instantiate(star,loc.Coord*100,new Quaternion(0,0,0,0)) as GameObject;
					    clone.name = name;
                        clone.GetComponentInChildren<StarCloseUp>().color = new Color(colorMat.r / 255, colorMat.g / 255, colorMat.b / 255);
                        float sizeUnLog = Mathf.Pow(10,((float)size)-4.85f);
                        clone.GetComponent<Star>().name = name;
                        clone.GetComponent<Star>().distance = loc.distance.ToString();
                        clone.GetComponent<Star>().specType = specType;
                        clone.GetComponent<Star>().otherName = otherName;
                        clone.GetComponent<Star>().remarks = remarks;

                        Vector3 sizeVec = new Vector3(((float)size)/48.5f, ((float)size)/ 48.5f, ((float)size) / 48.5f);
                        clone.transform.GetChild(0).localScale = sizeVec;
                        clone.transform.GetChild(0).GetComponent<Light>().range *= (float)size / 48.5f;
                        clone.transform.GetChild(0).GetChild(1).localScale = sizeVec;
                        clone.transform.GetChild(0).GetChild(1).GetComponent<Light>().range *= (float)size / 9.7f; 
                        clone.transform.GetChild(0).GetChild(0).localScale = sizeVec;


                    }
				}
			}
		}
	}
}