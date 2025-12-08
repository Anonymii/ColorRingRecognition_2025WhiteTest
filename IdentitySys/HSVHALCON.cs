//using System;
//using HalconDotNet;

//public partial class HDevelopExport
//{
//    public HTuple hv_ExpDefaultWinHandle;

//    // Procedures 
//    // Chapter: Develop
//    // Short Description: Open a new graphics window that preserves the aspect ratio of the given image. 
//    public void dev_open_window_fit_image(HObject ho_Image, HTuple hv_Row, HTuple hv_Column,
//        HTuple hv_WidthLimit, HTuple hv_HeightLimit, out HTuple hv_WindowHandle)
//    {




//        // Local iconic variables 

//        // Local control variables 

//        HTuple hv_MinWidth = new HTuple(), hv_MaxWidth = new HTuple();
//        HTuple hv_MinHeight = new HTuple(), hv_MaxHeight = new HTuple();
//        HTuple hv_ResizeFactor = new HTuple(), hv_ImageWidth = new HTuple();
//        HTuple hv_ImageHeight = new HTuple(), hv_TempWidth = new HTuple();
//        HTuple hv_TempHeight = new HTuple(), hv_WindowWidth = new HTuple();
//        HTuple hv_WindowHeight = new HTuple();
//        // Initialize local and output iconic variables 
//        hv_WindowHandle = new HTuple();
//        //This procedure opens a new graphics window and adjusts the size
//        //such that it fits into the limits specified by WidthLimit
//        //and HeightLimit, but also maintains the correct image aspect ratio.
//        //
//        //If it is impossible to match the minimum and maximum extent requirements
//        //at the same time (f.e. if the image is very long but narrow),
//        //the maximum value gets a higher priority,
//        //
//        //Parse input tuple WidthLimit
//        if ((int)((new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(0))).TupleOr(
//            new HTuple(hv_WidthLimit.TupleLess(0)))) != 0)
//        {
//            hv_MinWidth.Dispose();
//            hv_MinWidth = 500;
//            hv_MaxWidth.Dispose();
//            hv_MaxWidth = 800;
//        }
//        else if ((int)(new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(
//            1))) != 0)
//        {
//            hv_MinWidth.Dispose();
//            hv_MinWidth = 0;
//            hv_MaxWidth.Dispose();
//            hv_MaxWidth = new HTuple(hv_WidthLimit);
//        }
//        else
//        {
//            hv_MinWidth.Dispose();
//            using (HDevDisposeHelper dh = new HDevDisposeHelper())
//            {
//                hv_MinWidth = hv_WidthLimit.TupleSelect(
//                    0);
//            }
//            hv_MaxWidth.Dispose();
//            using (HDevDisposeHelper dh = new HDevDisposeHelper())
//            {
//                hv_MaxWidth = hv_WidthLimit.TupleSelect(
//                    1);
//            }
//        }
//        //Parse input tuple HeightLimit
//        if ((int)((new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(0))).TupleOr(
//            new HTuple(hv_HeightLimit.TupleLess(0)))) != 0)
//        {
//            hv_MinHeight.Dispose();
//            hv_MinHeight = 400;
//            hv_MaxHeight.Dispose();
//            hv_MaxHeight = 600;
//        }
//        else if ((int)(new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(
//            1))) != 0)
//        {
//            hv_MinHeight.Dispose();
//            hv_MinHeight = 0;
//            hv_MaxHeight.Dispose();
//            hv_MaxHeight = new HTuple(hv_HeightLimit);
//        }
//        else
//        {
//            hv_MinHeight.Dispose();
//            using (HDevDisposeHelper dh = new HDevDisposeHelper())
//            {
//                hv_MinHeight = hv_HeightLimit.TupleSelect(
//                    0);
//            }
//            hv_MaxHeight.Dispose();
//            using (HDevDisposeHelper dh = new HDevDisposeHelper())
//            {
//                hv_MaxHeight = hv_HeightLimit.TupleSelect(
//                    1);
//            }
//        }
//        //
//        //Test, if window size has to be changed.
//        hv_ResizeFactor.Dispose();
//        hv_ResizeFactor = 1;
//        hv_ImageWidth.Dispose(); hv_ImageHeight.Dispose();
//        HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
//        //First, expand window to the minimum extents (if necessary).
//        if ((int)((new HTuple(hv_MinWidth.TupleGreater(hv_ImageWidth))).TupleOr(new HTuple(hv_MinHeight.TupleGreater(
//            hv_ImageHeight)))) != 0)
//        {
//            hv_ResizeFactor.Dispose();
//            using (HDevDisposeHelper dh = new HDevDisposeHelper())
//            {
//                hv_ResizeFactor = (((((hv_MinWidth.TupleReal()
//                    ) / hv_ImageWidth)).TupleConcat((hv_MinHeight.TupleReal()) / hv_ImageHeight))).TupleMax()
//                    ;
//            }
//        }
//        hv_TempWidth.Dispose();
//        using (HDevDisposeHelper dh = new HDevDisposeHelper())
//        {
//            hv_TempWidth = hv_ImageWidth * hv_ResizeFactor;
//        }
//        hv_TempHeight.Dispose();
//        using (HDevDisposeHelper dh = new HDevDisposeHelper())
//        {
//            hv_TempHeight = hv_ImageHeight * hv_ResizeFactor;
//        }
//        //Then, shrink window to maximum extents (if necessary).
//        if ((int)((new HTuple(hv_MaxWidth.TupleLess(hv_TempWidth))).TupleOr(new HTuple(hv_MaxHeight.TupleLess(
//            hv_TempHeight)))) != 0)
//        {
//            using (HDevDisposeHelper dh = new HDevDisposeHelper())
//            {
//                {
//                    HTuple
//                      ExpTmpLocalVar_ResizeFactor = hv_ResizeFactor * ((((((hv_MaxWidth.TupleReal()
//                        ) / hv_TempWidth)).TupleConcat((hv_MaxHeight.TupleReal()) / hv_TempHeight))).TupleMin()
//                        );
//                    hv_ResizeFactor.Dispose();
//                    hv_ResizeFactor = ExpTmpLocalVar_ResizeFactor;
//                }
//            }
//        }
//        hv_WindowWidth.Dispose();
//        using (HDevDisposeHelper dh = new HDevDisposeHelper())
//        {
//            hv_WindowWidth = hv_ImageWidth * hv_ResizeFactor;
//        }
//        hv_WindowHeight.Dispose();
//        using (HDevDisposeHelper dh = new HDevDisposeHelper())
//        {
//            hv_WindowHeight = hv_ImageHeight * hv_ResizeFactor;
//        }
//        //Resize window
//        //dev_open_window(...);
//        using (HDevDisposeHelper dh = new HDevDisposeHelper())
//        {
//            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, hv_ImageHeight - 1, hv_ImageWidth - 1);
//        }

//        hv_MinWidth.Dispose();
//        hv_MaxWidth.Dispose();
//        hv_MinHeight.Dispose();
//        hv_MaxHeight.Dispose();
//        hv_ResizeFactor.Dispose();
//        hv_ImageWidth.Dispose();
//        hv_ImageHeight.Dispose();
//        hv_TempWidth.Dispose();
//        hv_TempHeight.Dispose();
//        hv_WindowWidth.Dispose();
//        hv_WindowHeight.Dispose();

//        return;
//    }

//    // Main procedure 
//    private string action()
//    {
//         // Local iconic variables 
//        string tubecolor;
//        HObject ho_Imageq, ho_Image, ho_ImageMirror;
//        HObject ho_Red, ho_Green, ho_Blue, ho_H, ho_S, ho_V, ho_RegionR1;
//        HObject ho_RegionRR1, ho_RegionG1, ho_RegionB1, ho_RegionY1;
//        HObject ho_RegionW1, ho_RegionUnionRed, ho_ImageReducedR1;
//        HObject ho_ImageReducedG1, ho_ImageReducedB1, ho_ImageReducedY1;
//        HObject ho_ImageReducedW1, ho_RegionR2, ho_RegionG2;
//        HObject ho_RegionB2, ho_RegionY2, ho_RegionW2, ho_ImageReducedR2;
//        HObject ho_ImageReducedG2, ho_ImageReducedB2, ho_ImageReducedY2;
//        HObject ho_ImageReducedW2, ho_RegionR3, ho_RegionG3;
//        HObject ho_RegionB3, ho_RegionY3, ho_RegionW3, ho_ImageReducedR3;
//        HObject ho_ImageReducedG3, ho_ImageReducedB3, ho_ImageReducedY3;
//        HObject ho_ImageReducedW3, ho_RegionFillUpredred, ho_RegionFillUp = null;
//        HObject ho_Image3;
//        // Local control variables 

//        HTuple hv_WindowHandle = new HTuple(), hv_AreaR = new HTuple();
//        HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
//        HTuple hv_AreaG = new HTuple(), hv_AreaB = new HTuple();
//        HTuple hv_AreaY = new HTuple(), hv_AreaW = new HTuple();
//        HTuple hv_RowR1 = new HTuple(), hv_ColumnR1 = new HTuple();
//        HTuple hv_RowR2 = new HTuple(), hv_ColumnR2 = new HTuple();
//        HTuple hv_cred1 = new HTuple(), hv_sred1 = new HTuple();
//        HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
//        HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
//        HTuple hv_swhite = new HTuple();
//        // Initialize local and output iconic variables 
//        HOperatorSet.GenEmptyObj(out ho_Imageq);
//        HOperatorSet.GenEmptyObj(out ho_Image);
//        HOperatorSet.GenEmptyObj(out ho_ImageMirror);
//        HOperatorSet.GenEmptyObj(out ho_Red);
//        HOperatorSet.GenEmptyObj(out ho_Green);
//        HOperatorSet.GenEmptyObj(out ho_Blue);
//        HOperatorSet.GenEmptyObj(out ho_H);
//        HOperatorSet.GenEmptyObj(out ho_S);
//        HOperatorSet.GenEmptyObj(out ho_V);
//        HOperatorSet.GenEmptyObj(out ho_RegionR1);
//        HOperatorSet.GenEmptyObj(out ho_RegionRR1);
//        HOperatorSet.GenEmptyObj(out ho_RegionG1);
//        HOperatorSet.GenEmptyObj(out ho_RegionB1);
//        HOperatorSet.GenEmptyObj(out ho_RegionY1);
//        HOperatorSet.GenEmptyObj(out ho_RegionW1);
//        HOperatorSet.GenEmptyObj(out ho_RegionUnionRed);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedR1);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedG1);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedB1);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedY1);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedW1);
//        HOperatorSet.GenEmptyObj(out ho_RegionR2);
//        HOperatorSet.GenEmptyObj(out ho_RegionG2);
//        HOperatorSet.GenEmptyObj(out ho_RegionB2);
//        HOperatorSet.GenEmptyObj(out ho_RegionY2);
//        HOperatorSet.GenEmptyObj(out ho_RegionW2);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedR2);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedG2);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedB2);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedY2);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedW2);
//        HOperatorSet.GenEmptyObj(out ho_RegionR3);
//        HOperatorSet.GenEmptyObj(out ho_RegionG3);
//        HOperatorSet.GenEmptyObj(out ho_RegionB3);
//        HOperatorSet.GenEmptyObj(out ho_RegionY3);
//        HOperatorSet.GenEmptyObj(out ho_RegionW3);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedR3);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedG3);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedB3);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedY3);
//        HOperatorSet.GenEmptyObj(out ho_ImageReducedW3);
//        HOperatorSet.GenEmptyObj(out ho_RegionFillUpredred);
//        HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
//        HOperatorSet.GenEmptyObj(out ho_Image3);
//        //*颜色识别HLS
//        //dev_close_window(...);
//        ho_Imageq.Dispose();
//        //HOperatorSet.ReadImage(out ho_Imageq, "E:/onedrive/桌面/图片/Sig_2021_03_17_11_48_28_840.JPG");
//        HOperatorSet.ReadImage(out ho_Imageq, IdentitySys.tubecolor.pathoftubepic);

//        ho_Image.Dispose();
//        HOperatorSet.Rectangle1Domain(ho_Imageq, out ho_Image, 272, 17, 748, 1256);
//        ho_ImageMirror.Dispose();
//        HOperatorSet.MirrorImage(ho_Imageq, out ho_ImageMirror, "row");
//        //转换为三通道图片
//        ho_Red.Dispose(); ho_Green.Dispose(); ho_Blue.Dispose();
//        HOperatorSet.Decompose3(ho_Image, out ho_Red, out ho_Green, out ho_Blue);
//        //将解离好的三通道图片作为传入然后输出hsv色调
//        //hls
//        ho_H.Dispose(); ho_S.Dispose(); ho_V.Dispose();
//        HOperatorSet.TransFromRgb(ho_Red, ho_Green, ho_Blue, out ho_H, out ho_S, out ho_V,
//            "hsv");
//        //色调H筛选
//        //threshold (H, Region, 180, 220)
//        ho_RegionR1.Dispose();
//        HOperatorSet.Threshold(ho_H, out ho_RegionR1, 240, 255);
//        ho_RegionRR1.Dispose();
//        HOperatorSet.Threshold(ho_H, out ho_RegionRR1, 0, 5);
//        ho_RegionG1.Dispose();
//        HOperatorSet.Threshold(ho_H, out ho_RegionG1, 50, 120);
//        ho_RegionB1.Dispose();
//        HOperatorSet.Threshold(ho_H, out ho_RegionB1, 145, 175);
//        //threshold (H, RegionY1, 16, 45)
//        ho_RegionY1.Dispose();
//        HOperatorSet.Threshold(ho_H, out ho_RegionY1, 21, 46);
//        ho_RegionW1.Dispose();
//        HOperatorSet.Threshold(ho_H, out ho_RegionW1, 0, 180);
//        ho_RegionUnionRed.Dispose();
//        HOperatorSet.Union2(ho_RegionR1, ho_RegionRR1, out ho_RegionUnionRed);
//        ho_ImageReducedR1.Dispose();
//        HOperatorSet.ReduceDomain(ho_Image, ho_RegionUnionRed, out ho_ImageReducedR1);
//        ho_ImageReducedG1.Dispose();
//        HOperatorSet.ReduceDomain(ho_Image, ho_RegionG1, out ho_ImageReducedG1);
//        ho_ImageReducedB1.Dispose();
//        HOperatorSet.ReduceDomain(ho_Image, ho_RegionB1, out ho_ImageReducedB1);
//        ho_ImageReducedY1.Dispose();
//        HOperatorSet.ReduceDomain(ho_Image, ho_RegionY1, out ho_ImageReducedY1);
//        ho_ImageReducedW1.Dispose();
//        HOperatorSet.ReduceDomain(ho_Image, ho_RegionW1, out ho_ImageReducedW1);
//        //亮度L筛选
//        ho_RegionR2.Dispose();
//        HOperatorSet.Threshold(ho_V, out ho_RegionR2, 50, 255);
//        ho_RegionG2.Dispose();
//        HOperatorSet.Threshold(ho_V, out ho_RegionG2, 40, 255);
//        //threshold (V, RegionB2, 25, 105)
//        ho_RegionB2.Dispose();
//        HOperatorSet.Threshold(ho_V, out ho_RegionB2, 45, 255);
//        ho_RegionY2.Dispose();
//        HOperatorSet.Threshold(ho_V, out ho_RegionY2, 45, 255);
//        //threshold (V, RegionW2, 221, 255)
//        ho_RegionW2.Dispose();
//        HOperatorSet.Threshold(ho_V, out ho_RegionW2, 140, 255);
//        ho_ImageReducedR2.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedR1, ho_RegionR2, out ho_ImageReducedR2
//            );
//        ho_ImageReducedG2.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedG1, ho_RegionG2, out ho_ImageReducedG2
//            );
//        ho_ImageReducedB2.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedB1, ho_RegionB2, out ho_ImageReducedB2
//            );
//        ho_ImageReducedY2.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedY1, ho_RegionY2, out ho_ImageReducedY2
//            );
//        ho_ImageReducedW2.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedW1, ho_RegionW2, out ho_ImageReducedW2
//            );
//        //饱和度S筛选
//        ho_RegionR3.Dispose();
//        HOperatorSet.Threshold(ho_S, out ho_RegionR3, 130, 255);
//        ho_RegionG3.Dispose();
//        HOperatorSet.Threshold(ho_S, out ho_RegionG3, 30, 255);
//        ho_RegionB3.Dispose();
//        HOperatorSet.Threshold(ho_S, out ho_RegionB3, 60, 255);
//        ho_RegionY3.Dispose();
//        HOperatorSet.Threshold(ho_S, out ho_RegionY3,55, 255);
//        ho_RegionW3.Dispose();
//        HOperatorSet.Threshold(ho_S, out ho_RegionW3, 0, 60);
//        hv_WindowHandle.Dispose();
//        dev_open_window_fit_image(ho_Imageq, 0, 0, -1, -1, out hv_WindowHandle);
//        HOperatorSet.DispObj(ho_ImageMirror, hv_ExpDefaultWinHandle);
//        ho_ImageReducedR3.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedR2, ho_RegionR3, out ho_ImageReducedR3
//            );
//        ho_ImageReducedG3.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedG2, ho_RegionG3, out ho_ImageReducedG3
//            );
//        ho_ImageReducedB3.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedB2, ho_RegionB3, out ho_ImageReducedB3
//            );
//        ho_ImageReducedY3.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedY2, ho_RegionY3, out ho_ImageReducedY3
//            );
//        ho_ImageReducedW3.Dispose();
//        HOperatorSet.ReduceDomain(ho_ImageReducedW2, ho_RegionW3, out ho_ImageReducedW3
//            );
//        hv_AreaR.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
//        HOperatorSet.AreaCenter(ho_ImageReducedR3, out hv_AreaR, out hv_Row, out hv_Column);
//        hv_AreaG.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
//        HOperatorSet.AreaCenter(ho_ImageReducedG3, out hv_AreaG, out hv_Row, out hv_Column);
//        hv_AreaB.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
//        HOperatorSet.AreaCenter(ho_ImageReducedB3, out hv_AreaB, out hv_Row, out hv_Column);
//        hv_AreaY.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
//        HOperatorSet.AreaCenter(ho_ImageReducedY3, out hv_AreaY, out hv_Row, out hv_Column);
//        hv_AreaW.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
//        HOperatorSet.AreaCenter(ho_ImageReducedW3, out hv_AreaW, out hv_Row, out hv_Column);
//        ho_RegionFillUpredred.Dispose();
//        HOperatorSet.FillUp(ho_ImageReducedR3, out ho_RegionFillUpredred);
//        hv_RowR1.Dispose(); hv_ColumnR1.Dispose(); hv_RowR2.Dispose(); hv_ColumnR2.Dispose();
//        HOperatorSet.InnerRectangle1(ho_RegionFillUpredred, out hv_RowR1, out hv_ColumnR1,
//            out hv_RowR2, out hv_ColumnR2);
//        hv_cred1.Dispose();
//        using (HDevDisposeHelper dh = new HDevDisposeHelper())
//        {
//            hv_cred1 = hv_ColumnR2 - hv_ColumnR1;
//        }
//        hv_sred1.Dispose();
//        using (HDevDisposeHelper dh = new HDevDisposeHelper())
//        {
//            hv_sred1 = hv_RowR2 - hv_RowR1;
//        }
//        if ((int)((new HTuple(hv_AreaR.TupleGreater(11000))).TupleAnd((new HTuple(hv_sred1.TupleGreater(
//         40))).TupleOr(new HTuple(hv_cred1.TupleGreater(40))))) != 0)
//        {
//            HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为红色", "window", "center",
//                "center", "black", new HTuple(), new HTuple());
//            //color := 'red'
//            tubecolor = "Red";
//        }
//        else if ((int)(new HTuple(hv_AreaG.TupleGreater(20000))) != 0)
//        {
//            HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为绿色", "window", "center",
//                "center", "black", new HTuple(), new HTuple());
//            //color := 'green''
//            tubecolor = "Green";
//        }
//        else if ((int)(new HTuple(hv_AreaB.TupleGreater(30000))) != 0)
//        {
//            HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为蓝色", "window", "center",
//                "center", "black", new HTuple(), new HTuple());
//            //color := 'blue'
//            tubecolor = "Blue";
//        }
//        else if ((int)(new HTuple(hv_AreaY.TupleGreater(20000))) != 0)
//        {
//            HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为黄色", "window", "center",
//                "center", "black", new HTuple(), new HTuple());
//            tubecolor = "Yellow";
//            //color := 'yellow'
//        }
//        else if ((int)(new HTuple(hv_AreaW.TupleGreater(150000))) != 0)
//        {
//            ho_RegionFillUp.Dispose();
//            HOperatorSet.FillUp(ho_ImageReducedW3, out ho_RegionFillUp);
//            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
//            HOperatorSet.InnerRectangle1(ho_RegionFillUp, out hv_Row1, out hv_Column1,
//                out hv_Row2, out hv_Column2);
//            hv_swhite.Dispose();
//            using (HDevDisposeHelper dh = new HDevDisposeHelper())
//            {
//                hv_swhite = hv_Row2 - hv_Row1;
//            }
//            if ((int)(new HTuple(hv_swhite.TupleGreater(100))) != 0)
//            {
//                HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为白色", "window", "center",
//                "center", "black", new HTuple(), new HTuple());
//                //color := 'white'
//                tubecolor = "White";
//            }
//            else
//            {
//                tubecolor = null;
//            }
//        }
//        else
//        {
//            //HOperatorSet.DispText(hv_ExpDefaultWinHandle, "未检测到色环，请手动识别",
//            //    "window", "center", "center", "black", new HTuple(), new HTuple());
//            tubecolor =null;
//        }
//        ho_Image3.Dispose();
//        HOperatorSet.DumpWindowImage(out ho_Image3, hv_ExpDefaultWinHandle);
//        if (tubecolor != null)//未检测到的不存
//        {
//            HOperatorSet.WriteImage(ho_Image3, "jpg", 0, IdentitySys.tubecolor.savecolorpic);
//        }


//        //HOperatorSet.WriteImage(ho_Image3, "jpg", 0, "D:/色环识别/处理后图像/3.15");

//        ho_Imageq.Dispose();
//        ho_Image.Dispose();
//        ho_ImageMirror.Dispose();
//        ho_Red.Dispose();
//        ho_Green.Dispose();
//        ho_Blue.Dispose();
//        ho_H.Dispose();
//        ho_S.Dispose();
//        ho_V.Dispose();
//        ho_RegionR1.Dispose();
//        ho_RegionRR1.Dispose();
//        ho_RegionG1.Dispose();
//        ho_RegionB1.Dispose();
//        ho_RegionY1.Dispose();
//        ho_RegionW1.Dispose();
//        ho_RegionUnionRed.Dispose();
//        ho_ImageReducedR1.Dispose();
//        ho_ImageReducedG1.Dispose();
//        ho_ImageReducedB1.Dispose();
//        ho_ImageReducedY1.Dispose();
//        ho_ImageReducedW1.Dispose();
//        ho_RegionR2.Dispose();
//        ho_RegionG2.Dispose();
//        ho_RegionB2.Dispose();
//        ho_RegionY2.Dispose();
//        ho_RegionW2.Dispose();
//        ho_ImageReducedR2.Dispose();
//        ho_ImageReducedG2.Dispose();
//        ho_ImageReducedB2.Dispose();
//        ho_ImageReducedY2.Dispose();
//        ho_ImageReducedW2.Dispose();
//        ho_RegionR3.Dispose();
//        ho_RegionG3.Dispose();
//        ho_RegionB3.Dispose();
//        ho_RegionY3.Dispose();
//        ho_RegionW3.Dispose();
//        ho_ImageReducedR3.Dispose();
//        ho_ImageReducedG3.Dispose();
//        ho_ImageReducedB3.Dispose();
//        ho_ImageReducedY3.Dispose();
//        ho_ImageReducedW3.Dispose();
//        ho_RegionFillUpredred.Dispose();
//        ho_RegionFillUp.Dispose();
//        ho_Image3.Dispose();

//        hv_WindowHandle.Dispose();
//        hv_AreaR.Dispose();
//        hv_Row.Dispose();
//        hv_Column.Dispose();
//        hv_AreaG.Dispose();
//        hv_AreaB.Dispose();
//        hv_AreaY.Dispose();
//        hv_AreaW.Dispose();
//        hv_RowR1.Dispose();
//        hv_ColumnR1.Dispose();
//        hv_RowR2.Dispose();
//        hv_ColumnR2.Dispose();
//        hv_cred1.Dispose();
//        hv_sred1.Dispose();
//        hv_Row1.Dispose();
//        hv_Column1.Dispose();
//        hv_Row2.Dispose();
//        hv_Column2.Dispose();
//        hv_swhite.Dispose();
//        return tubecolor;
//    }

//    public void InitHalcon()
//    {
//        // Default settings used in HDevelop
//        HOperatorSet.SetSystem("width", 512);
//        HOperatorSet.SetSystem("height", 512);
//    }

//    public string RunHalcon(HTuple Window)
//    {
//        string Scolor;
//        hv_ExpDefaultWinHandle = Window;
//        Scolor = action();
//        return Scolor;
//    }

//}
using System;
using HalconDotNet;

public partial class HDevelopExport
{
    public HTuple hv_ExpDefaultWinHandle;
    private int Step = 0;
    // Procedures 
    // Chapter: Develop
    // Short Description: Open a new graphics window that preserves the aspect ratio of the given image. 
    public void dev_open_window_fit_image(HObject ho_Image, HTuple hv_Row, HTuple hv_Column,
        HTuple hv_WidthLimit, HTuple hv_HeightLimit, out HTuple hv_WindowHandle)
    {




        // Local iconic variables 

        // Local control variables 

        HTuple hv_MinWidth = new HTuple(), hv_MaxWidth = new HTuple();
        HTuple hv_MinHeight = new HTuple(), hv_MaxHeight = new HTuple();
        HTuple hv_ResizeFactor = new HTuple(), hv_ImageWidth = new HTuple();
        HTuple hv_ImageHeight = new HTuple(), hv_TempWidth = new HTuple();
        HTuple hv_TempHeight = new HTuple(), hv_WindowWidth = new HTuple();
        HTuple hv_WindowHeight = new HTuple();
        // Initialize local and output iconic variables 
        hv_WindowHandle = new HTuple();
        //This procedure opens a new graphics window and adjusts the size
        //such that it fits into the limits specified by WidthLimit
        //and HeightLimit, but also maintains the correct image aspect ratio.
        //
        //If it is impossible to match the minimum and maximum extent requirements
        //at the same time (f.e. if the image is very long but narrow),
        //the maximum value gets a higher priority,
        //
        //Parse input tuple WidthLimit
        if ((int)((new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(0))).TupleOr(
            new HTuple(hv_WidthLimit.TupleLess(0)))) != 0)
        {
            hv_MinWidth.Dispose();
            hv_MinWidth = 500;
            hv_MaxWidth.Dispose();
            hv_MaxWidth = 800;
        }
        else if ((int)(new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(
            1))) != 0)
        {
            hv_MinWidth.Dispose();
            hv_MinWidth = 0;
            hv_MaxWidth.Dispose();
            hv_MaxWidth = new HTuple(hv_WidthLimit);
        }
        else
        {
            hv_MinWidth.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_MinWidth = hv_WidthLimit.TupleSelect(
                    0);
            }
            hv_MaxWidth.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_MaxWidth = hv_WidthLimit.TupleSelect(
                    1);
            }
        }
        //Parse input tuple HeightLimit
        if ((int)((new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(0))).TupleOr(
            new HTuple(hv_HeightLimit.TupleLess(0)))) != 0)
        {
            hv_MinHeight.Dispose();
            hv_MinHeight = 400;
            hv_MaxHeight.Dispose();
            hv_MaxHeight = 600;
        }
        else if ((int)(new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(
            1))) != 0)
        {
            hv_MinHeight.Dispose();
            hv_MinHeight = 0;
            hv_MaxHeight.Dispose();
            hv_MaxHeight = new HTuple(hv_HeightLimit);
        }
        else
        {
            hv_MinHeight.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_MinHeight = hv_HeightLimit.TupleSelect(
                    0);
            }
            hv_MaxHeight.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_MaxHeight = hv_HeightLimit.TupleSelect(
                    1);
            }
        }
        //
        //Test, if window size has to be changed.
        hv_ResizeFactor.Dispose();
        hv_ResizeFactor = 1;
        hv_ImageWidth.Dispose(); hv_ImageHeight.Dispose();
        HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
        //First, expand window to the minimum extents (if necessary).
        if ((int)((new HTuple(hv_MinWidth.TupleGreater(hv_ImageWidth))).TupleOr(new HTuple(hv_MinHeight.TupleGreater(
            hv_ImageHeight)))) != 0)
        {
            hv_ResizeFactor.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ResizeFactor = (((((hv_MinWidth.TupleReal()
                    ) / hv_ImageWidth)).TupleConcat((hv_MinHeight.TupleReal()) / hv_ImageHeight))).TupleMax()
                    ;
            }
        }
        hv_TempWidth.Dispose();
        using (HDevDisposeHelper dh = new HDevDisposeHelper())
        {
            hv_TempWidth = hv_ImageWidth * hv_ResizeFactor;
        }
        hv_TempHeight.Dispose();
        using (HDevDisposeHelper dh = new HDevDisposeHelper())
        {
            hv_TempHeight = hv_ImageHeight * hv_ResizeFactor;
        }
        //Then, shrink window to maximum extents (if necessary).
        if ((int)((new HTuple(hv_MaxWidth.TupleLess(hv_TempWidth))).TupleOr(new HTuple(hv_MaxHeight.TupleLess(
            hv_TempHeight)))) != 0)
        {
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_ResizeFactor = hv_ResizeFactor * ((((((hv_MaxWidth.TupleReal()
                        ) / hv_TempWidth)).TupleConcat((hv_MaxHeight.TupleReal()) / hv_TempHeight))).TupleMin()
                        );
                    hv_ResizeFactor.Dispose();
                    hv_ResizeFactor = ExpTmpLocalVar_ResizeFactor;
                }
            }
        }
        hv_WindowWidth.Dispose();
        using (HDevDisposeHelper dh = new HDevDisposeHelper())
        {
            hv_WindowWidth = hv_ImageWidth * hv_ResizeFactor;
        }
        hv_WindowHeight.Dispose();
        using (HDevDisposeHelper dh = new HDevDisposeHelper())
        {
            hv_WindowHeight = hv_ImageHeight * hv_ResizeFactor;
        }
        //Resize window
        //dev_open_window(...);
        using (HDevDisposeHelper dh = new HDevDisposeHelper())
        {
            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, hv_ImageHeight - 1, hv_ImageWidth - 1);
        }

        hv_MinWidth.Dispose();
        hv_MaxWidth.Dispose();
        hv_MinHeight.Dispose();
        hv_MaxHeight.Dispose();
        hv_ResizeFactor.Dispose();
        hv_ImageWidth.Dispose();
        hv_ImageHeight.Dispose();
        hv_TempWidth.Dispose();
        hv_TempHeight.Dispose();
        hv_WindowWidth.Dispose();
        hv_WindowHeight.Dispose();

        return;
    }

    // Main procedure 
    private string action(string CamSavepath, string ProcessSavePath)
    {
        // Local iconic variables 
        string tubecolor = null;
        HObject ho_Imageq, ho_Image, ho_ImageMirror;
        HObject ho_Red, ho_Green, ho_Blue, ho_H, ho_S, ho_V, ho_RegionR1;
        HObject ho_RegionRR1, ho_RegionG1, ho_RegionB1, ho_RegionY1;
        HObject ho_RegionW1, ho_RegionUnionRed, ho_ImageReducedR1;
        HObject ho_ImageReducedG1, ho_ImageReducedB1, ho_ImageReducedY1;
        HObject ho_ImageReducedW1, ho_RegionR2, ho_RegionG2;
        HObject ho_RegionB2, ho_RegionY2, ho_RegionW2, ho_ImageReducedR2;
        HObject ho_ImageReducedG2, ho_ImageReducedB2, ho_ImageReducedY2;
        HObject ho_ImageReducedW2, ho_RegionR3, ho_RegionG3;
        HObject ho_RegionB3, ho_RegionY3, ho_RegionW3, ho_ImageReducedR3;
        HObject ho_ImageReducedG3, ho_ImageReducedB3, ho_ImageReducedY3;
        HObject ho_ImageReducedW3, ho_RegionFillUpredred, ho_RegionFillUp = null;
        HObject ho_Image3;
        // Local control variables 

        HTuple hv_WindowHandle = new HTuple(), hv_AreaR = new HTuple();
        HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
        HTuple hv_AreaG = new HTuple(), hv_AreaB = new HTuple();
        HTuple hv_AreaY = new HTuple(), hv_AreaW = new HTuple();
        HTuple hv_RowR1 = new HTuple(), hv_ColumnR1 = new HTuple();
        HTuple hv_RowR2 = new HTuple(), hv_ColumnR2 = new HTuple();
        HTuple hv_cred1 = new HTuple(), hv_sred1 = new HTuple();
        HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
        HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
        HTuple hv_swhite = new HTuple();
        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Imageq);
        HOperatorSet.GenEmptyObj(out ho_Image);
        HOperatorSet.GenEmptyObj(out ho_ImageMirror);
        HOperatorSet.GenEmptyObj(out ho_Red);
        HOperatorSet.GenEmptyObj(out ho_Green);
        HOperatorSet.GenEmptyObj(out ho_Blue);
        HOperatorSet.GenEmptyObj(out ho_H);
        HOperatorSet.GenEmptyObj(out ho_S);
        HOperatorSet.GenEmptyObj(out ho_V);
        HOperatorSet.GenEmptyObj(out ho_RegionR1);
        HOperatorSet.GenEmptyObj(out ho_RegionRR1);
        HOperatorSet.GenEmptyObj(out ho_RegionG1);
        HOperatorSet.GenEmptyObj(out ho_RegionB1);
        HOperatorSet.GenEmptyObj(out ho_RegionY1);
        HOperatorSet.GenEmptyObj(out ho_RegionW1);
        HOperatorSet.GenEmptyObj(out ho_RegionUnionRed);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedR1);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedG1);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedB1);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedY1);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedW1);
        HOperatorSet.GenEmptyObj(out ho_RegionR2);
        HOperatorSet.GenEmptyObj(out ho_RegionG2);
        HOperatorSet.GenEmptyObj(out ho_RegionB2);
        HOperatorSet.GenEmptyObj(out ho_RegionY2);
        HOperatorSet.GenEmptyObj(out ho_RegionW2);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedR2);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedG2);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedB2);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedY2);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedW2);
        HOperatorSet.GenEmptyObj(out ho_RegionR3);
        HOperatorSet.GenEmptyObj(out ho_RegionG3);
        HOperatorSet.GenEmptyObj(out ho_RegionB3);
        HOperatorSet.GenEmptyObj(out ho_RegionY3);
        HOperatorSet.GenEmptyObj(out ho_RegionW3);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedR3);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedG3);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedB3);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedY3);
        HOperatorSet.GenEmptyObj(out ho_ImageReducedW3);
        HOperatorSet.GenEmptyObj(out ho_RegionFillUpredred);
        HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
        HOperatorSet.GenEmptyObj(out ho_Image3);

        /***************************************************************************************************/
        // 新增调试用变量声明
        HObject ho_ImageWithHRegion, ho_ImageWithVRegion, ho_ImageWithSRegion;
        HObject ho_ImageWithFinalRegion, ho_Rectangle, ho_DebugImage;
        // 新增调试用HTuple变量声明
        HTuple hv_AreaH = new HTuple(), hv_RowH = new HTuple(), hv_ColumnH = new HTuple();
        HTuple hv_AreaV = new HTuple(), hv_RowV = new HTuple(), hv_ColumnV = new HTuple();
        HTuple hv_AreaS = new HTuple(), hv_RowS = new HTuple(), hv_ColumnS = new HTuple();
        HTuple hv_wwhite = new HTuple();

        // 调试模式开关
        bool debugMode = false;

        // 初始化调试用变量
        HOperatorSet.GenEmptyObj(out ho_ImageWithHRegion);
        HOperatorSet.GenEmptyObj(out ho_ImageWithVRegion);
        HOperatorSet.GenEmptyObj(out ho_ImageWithSRegion);
        HOperatorSet.GenEmptyObj(out ho_ImageWithFinalRegion);
        HOperatorSet.GenEmptyObj(out ho_Rectangle);
        HOperatorSet.GenEmptyObj(out ho_DebugImage);
        /***************************************************************************************************/

        //*颜色识别HLS
        //dev_close_window(...);
        ho_Imageq.Dispose();
        //HOperatorSet.ReadImage(out ho_Imageq, "E:/onedrive/桌面/图片/Sig_2021_03_17_11_48_28_840.JPG");
        HOperatorSet.ReadImage(out ho_Imageq, CamSavepath);
        ho_Image.Dispose();

        /***************************************************************************************************/

        //ROI选取
        //HOperatorSet.Rectangle1Domain(ho_Imageq, out ho_Image, 272, 17, 748, 1000);
        HOperatorSet.Rectangle1Domain(ho_Imageq, out ho_Image, 1, 1, 1000, 1000);

        //图片镜像
        //ho_ImageMirror.Dispose();
        //HOperatorSet.MirrorImage(ho_Imageq, out ho_ImageMirror, "row");

        /***************************************************************************************************/

        //转换为三通道图片
        ho_Red.Dispose(); ho_Green.Dispose(); ho_Blue.Dispose();
        HOperatorSet.Decompose3(ho_Image, out ho_Red, out ho_Green, out ho_Blue);
        //将解离好的三通道图片作为传入然后输出hsv色调
        //hls
        ho_H.Dispose(); ho_S.Dispose(); ho_V.Dispose();
        HOperatorSet.TransFromRgb(ho_Red, ho_Green, ho_Blue, out ho_H, out ho_S, out ho_V,
            "hsv");
        //色调H筛选
        hv_WindowHandle.Dispose();
        dev_open_window_fit_image(ho_Imageq, 0, 0, -1, -1, out hv_WindowHandle);

        //显示原图/镜像图
        HOperatorSet.DispObj(ho_Imageq, hv_ExpDefaultWinHandle);
        //HOperatorSet.DispObj(ho_ImageMirror, hv_ExpDefaultWinHandle);

        if (Step == 0)
        {
            ho_RegionR1.Dispose();
            HOperatorSet.Threshold(ho_H, out ho_RegionR1, 240, 255);
            ho_RegionRR1.Dispose();
            HOperatorSet.Threshold(ho_H, out ho_RegionRR1, 0, 5);
            ho_RegionUnionRed.Dispose();
            HOperatorSet.Union2(ho_RegionR1, ho_RegionRR1, out ho_RegionUnionRed);
            ho_ImageReducedR1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionUnionRed, out ho_ImageReducedR1);
            ho_RegionR2.Dispose();
            HOperatorSet.Threshold(ho_V, out ho_RegionR2, 50, 255);
            ho_ImageReducedR2.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedR1, ho_RegionR2, out ho_ImageReducedR2);
            ho_RegionR3.Dispose();
            HOperatorSet.Threshold(ho_S, out ho_RegionR3, 130, 255);
            ho_ImageReducedR3.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedR2, ho_RegionR3, out ho_ImageReducedR3
                );
            hv_AreaR.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_ImageReducedR3, out hv_AreaR, out hv_Row, out hv_Column);
            ho_RegionFillUpredred.Dispose();
            HOperatorSet.FillUp(ho_ImageReducedR3, out ho_RegionFillUpredred);
            hv_RowR1.Dispose(); hv_ColumnR1.Dispose(); hv_RowR2.Dispose(); hv_ColumnR2.Dispose();
            HOperatorSet.InnerRectangle1(ho_RegionFillUpredred, out hv_RowR1, out hv_ColumnR1,
                out hv_RowR2, out hv_ColumnR2);
            hv_cred1.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_cred1 = hv_ColumnR2 - hv_ColumnR1;
            }
            hv_sred1.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_sred1 = hv_RowR2 - hv_RowR1;
            }
            if ((int)((new HTuple(hv_AreaR.TupleGreater(11000))).TupleAnd((new HTuple(hv_sred1.TupleGreater(
     40))).TupleOr(new HTuple(hv_cred1.TupleGreater(40))))) != 0)
            {
                HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为红色", "window", "center",
                    "center", "black", new HTuple(), new HTuple());
                //color := 'red'
                tubecolor = "Red";
                Step = 10;
            }
            else
            {
                Step = 1;
            }
        }
        //if (Step == 1)
        //{
        //    // 白色环识别
        //    ho_RegionW1.Dispose();
        //    HOperatorSet.Threshold(ho_H, out ho_RegionW1, 0, 255);
        //    ho_ImageReducedW1.Dispose();
        //    HOperatorSet.ReduceDomain(ho_Image, ho_RegionW1, out ho_ImageReducedW1);

        //    // 亮度V筛选
        //    ho_RegionW2.Dispose();
        //    HOperatorSet.Threshold(ho_V, out ho_RegionW2, 60, 255);
        //    ho_ImageReducedW2.Dispose();
        //    HOperatorSet.ReduceDomain(ho_ImageReducedW1, ho_RegionW2, out ho_ImageReducedW2);

        //    // 饱和度S筛选
        //    ho_RegionW3.Dispose();
        //    HOperatorSet.Threshold(ho_S, out ho_RegionW3, 0, 60);
        //    ho_ImageReducedW3.Dispose();
        //    HOperatorSet.ReduceDomain(ho_ImageReducedW2, ho_RegionW3, out ho_ImageReducedW3);

        //    hv_AreaW.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
        //    HOperatorSet.AreaCenter(ho_ImageReducedW3, out hv_AreaW, out hv_Row, out hv_Column);


        //    if ((int)(new HTuple(hv_AreaW.TupleGreater(10000))) != 0)
        //    {
        //        ho_RegionFillUp.Dispose();
        //        HOperatorSet.FillUp(ho_ImageReducedW3, out ho_RegionFillUp);
        //        hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
        //        HOperatorSet.InnerRectangle1(ho_RegionFillUp, out hv_Row1, out hv_Column1,
        //            out hv_Row2, out hv_Column2);
        //        hv_swhite.Dispose();
        //        using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //        {
        //            hv_swhite = hv_Row2 - hv_Row1;
        //        }
        //        if ((int)(new HTuple(hv_swhite.TupleGreater(50))) != 0)
        //        {
        //            HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为白色", "window", "center",
        //            "center", "black", new HTuple(), new HTuple());
        //        tubecolor = "White";
        //        Step = 10;
        //        }
        //    }
        //    else
        //    {
        //        Step = 2;
        //    }
        //}

        if (Step == 1)
        {
            // 白色环识别
            // 色域H筛选
            ho_RegionW1.Dispose();
            HOperatorSet.Threshold(ho_H, out ho_RegionW1, 0, 255);
            ho_ImageReducedW1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionW1, out ho_ImageReducedW1);

            // 亮度V筛选
            ho_RegionW2.Dispose();
            HOperatorSet.Threshold(ho_V, out ho_RegionW2, 110, 255);
            ho_ImageReducedW2.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedW1, ho_RegionW2, out ho_ImageReducedW2);

            // 饱和度S筛选
            ho_RegionW3.Dispose();
            HOperatorSet.Threshold(ho_S, out ho_RegionW3, 0, 60);
            ho_ImageReducedW3.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedW2, ho_RegionW3, out ho_ImageReducedW3);

            // 计算最终区域面积和位置
            hv_AreaW.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_ImageReducedW3, out hv_AreaW, out hv_Row, out hv_Column);

            if (debugMode)
            {
                // 调试：显示最终合并区域
                HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "white");
                HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "margin");
                HOperatorSet.DispRegion(ho_ImageReducedW3, hv_ExpDefaultWinHandle);

                // 注意：ho_ImageReducedW3 是一个图像（通过ReduceDomain得到的），不是区域
                // 不能使用WriteRegion保存，使用WriteImage保存
                // 或者将图像转换为区域再保存

                // 方法1：直接保存图像
                HOperatorSet.WriteImage(ho_ImageReducedW3, "bmp", 0, "debug_final_white_image.bmp");

                // 显示最终区域信息
                HOperatorSet.DispText(hv_ExpDefaultWinHandle, $"最终白色候选区域面积: {hv_AreaW.I}",
                                      "window", "top", "left", "white", "box", "true");

            }

            // 检查面积阈值
            if ((int)(new HTuple(hv_AreaW.TupleGreater(12000))) != 0)
            {

                HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为白色", "window", "center",
                    "center", "black", new HTuple(), new HTuple());
                tubecolor = "White";
                Step = 10;

            }
            else
            {
                Step = 2;
            }
        }


        if (Step == 2)
        {
            ho_RegionB1.Dispose();
            HOperatorSet.Threshold(ho_H, out ho_RegionB1, 145, 175);
            ho_ImageReducedB1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionB1, out ho_ImageReducedB1);
            ho_RegionB2.Dispose();
            HOperatorSet.Threshold(ho_V, out ho_RegionB2, 45, 255);
            ho_ImageReducedB2.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedB1, ho_RegionB2, out ho_ImageReducedB2
                );
            ho_RegionB3.Dispose();
            HOperatorSet.Threshold(ho_S, out ho_RegionB3, 60, 255);
            ho_ImageReducedB3.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedB2, ho_RegionB3, out ho_ImageReducedB3
                );
            hv_AreaB.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_ImageReducedB3, out hv_AreaB, out hv_Row, out hv_Column);
            if ((int)(new HTuple(hv_AreaB.TupleGreater(30000))) != 0)
            {
                HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为蓝色", "window", "center",
                    "center", "black", new HTuple(), new HTuple());
                //color := 'blue'
                //tubecolor = "Blue";
                Step = 10;
            }
            else
            {
                Step = 3;
            }
        }
        if (Step == 3)
        {
            ho_RegionY1.Dispose();
            HOperatorSet.Threshold(ho_H, out ho_RegionY1, 21, 46);
            ho_ImageReducedY1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionY1, out ho_ImageReducedY1);
            ho_RegionY2.Dispose();
            HOperatorSet.Threshold(ho_V, out ho_RegionY2, 45, 255);
            ho_ImageReducedY2.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedY1, ho_RegionY2, out ho_ImageReducedY2
                );
            ho_RegionY3.Dispose();
            HOperatorSet.Threshold(ho_S, out ho_RegionY3, 55, 255);
            ho_ImageReducedY3.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReducedY2, ho_RegionY3, out ho_ImageReducedY3
                );

            hv_AreaY.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_ImageReducedY3, out hv_AreaY, out hv_Row, out hv_Column);
            if ((int)(new HTuple(hv_AreaY.TupleGreater(20000))) != 0)
            {
                HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为黄色", "window", "center",
                    "center", "black", new HTuple(), new HTuple());
                tubecolor = "Yellow";
                Step = 10;
            }
            else
            {
                tubecolor = null;
                Step = 10;
            }
        }

        //ho_RegionW1.Dispose();
        //HOperatorSet.Threshold(ho_H, out ho_RegionW1, 0, 180);
        //ho_ImageReducedW1.Dispose();
        //HOperatorSet.ReduceDomain(ho_Image, ho_RegionW1, out ho_ImageReducedW1);
        ////亮度L筛选
        //ho_RegionW2.Dispose();
        //HOperatorSet.Threshold(ho_V, out ho_RegionW2, 140, 255);
        //ho_ImageReducedW2.Dispose();
        //HOperatorSet.ReduceDomain(ho_ImageReducedW1, ho_RegionW2, out ho_ImageReducedW2
        //    );
        ////饱和度S筛选
        //ho_RegionW3.Dispose();
        //HOperatorSet.Threshold(ho_S, out ho_RegionW3, 0, 60);
        //ho_ImageReducedW3.Dispose();
        //HOperatorSet.ReduceDomain(ho_ImageReducedW2, ho_RegionW3, out ho_ImageReducedW3
        //    );
        //hv_AreaW.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
        //HOperatorSet.AreaCenter(ho_ImageReducedW3, out hv_AreaW, out hv_Row, out hv_Column);
        //if ((int)(new HTuple(hv_AreaW.TupleGreater(150000))) != 0)
        //{
        //    ho_RegionFillUp.Dispose();
        //    HOperatorSet.FillUp(ho_ImageReducedW3, out ho_RegionFillUp);
        //    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
        //    HOperatorSet.InnerRectangle1(ho_RegionFillUp, out hv_Row1, out hv_Column1,
        //        out hv_Row2, out hv_Column2);
        //    hv_swhite.Dispose();
        //    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //    {
        //        hv_swhite = hv_Row2 - hv_Row1;
        //    }
        //    if ((int)(new HTuple(hv_s      white.TupleGreater(100))) != 0)
        //    {
        //        HOperatorSet.DispText(hv_ExpDefaultWinHandle, "色环为白色", "window", "center",
        //        "center", "black", new HTuple(), new HTuple());
        //        //color := 'white'
        //        tubecolor = "White";
        //    }
        //    else
        //    {
        //        tubecolor = null;
        //    }
        //}
        //else
        //{
        //    //HOperatorSet.DispText(hv_ExpDefaultWinHandle, "未检测到色环，请手动识别",
        //    //    "window", "center", "center", "black", new HTuple(), new HTuple());
        //    tubecolor =null;
        //}
        if (Step == 10)
        {
            ho_Image3.Dispose();
            HOperatorSet.DumpWindowImage(out ho_Image3, hv_ExpDefaultWinHandle);
            if (tubecolor != null)//未检测到的不存
            {
                HOperatorSet.WriteImage(ho_Image3, "jpg", 0, ProcessSavePath);
            }


            //HOperatorSet.WriteImage(ho_Image3, "jpg", 0, "D:/色环识别/处理后图像/3.15");

            ho_Imageq.Dispose();
            ho_Image.Dispose();
            ho_ImageMirror.Dispose();
            ho_Red.Dispose();
            ho_Green.Dispose();
            ho_Blue.Dispose();
            ho_H.Dispose();
            ho_S.Dispose();
            ho_V.Dispose();
            ho_RegionR1.Dispose();
            ho_RegionRR1.Dispose();
            ho_RegionG1.Dispose();
            ho_RegionB1.Dispose();
            ho_RegionY1.Dispose();
            ho_RegionW1.Dispose();
            ho_RegionUnionRed.Dispose();
            ho_ImageReducedR1.Dispose();
            ho_ImageReducedG1.Dispose();
            ho_ImageReducedB1.Dispose();
            ho_ImageReducedY1.Dispose();
            ho_ImageReducedW1.Dispose();
            ho_RegionR2.Dispose();
            ho_RegionG2.Dispose();
            ho_RegionB2.Dispose();
            ho_RegionY2.Dispose();
            ho_RegionW2.Dispose();
            ho_ImageReducedR2.Dispose();
            ho_ImageReducedG2.Dispose();
            ho_ImageReducedB2.Dispose();
            ho_ImageReducedY2.Dispose();
            ho_ImageReducedW2.Dispose();
            ho_RegionR3.Dispose();
            ho_RegionG3.Dispose();
            ho_RegionB3.Dispose();
            ho_RegionY3.Dispose();
            ho_RegionW3.Dispose();
            ho_ImageReducedR3.Dispose();
            ho_ImageReducedG3.Dispose();
            ho_ImageReducedB3.Dispose();
            ho_ImageReducedY3.Dispose();
            ho_ImageReducedW3.Dispose();
            ho_RegionFillUpredred.Dispose();
            ho_RegionFillUp.Dispose();
            ho_Image3.Dispose();

            hv_WindowHandle.Dispose();
            hv_AreaR.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_AreaG.Dispose();
            hv_AreaB.Dispose();
            hv_AreaY.Dispose();
            hv_AreaW.Dispose();
            hv_RowR1.Dispose();
            hv_ColumnR1.Dispose();
            hv_RowR2.Dispose();
            hv_ColumnR2.Dispose();
            hv_cred1.Dispose();
            hv_sred1.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_swhite.Dispose();

            // 清理调试用HTuple变量
            hv_AreaH.Dispose();
            hv_RowH.Dispose();
            hv_ColumnH.Dispose();
            hv_AreaV.Dispose();
            hv_RowV.Dispose();
            hv_ColumnV.Dispose();
            hv_AreaS.Dispose();
            hv_RowS.Dispose();
            hv_ColumnS.Dispose();
            hv_wwhite.Dispose();

            Step = 0;
        }

        return tubecolor;
    }

    public void InitHalcon()
    {
        // Default settings used in HDevelop
        HOperatorSet.SetSystem("width", 512);
        HOperatorSet.SetSystem("height", 512);
    }

    public string RunHalcon(HTuple Window, string CamPath, string SavePath)
    {
        string Scolor;
        hv_ExpDefaultWinHandle = Window;
        Scolor = action(CamPath, SavePath);
        return Scolor;
    }

}


