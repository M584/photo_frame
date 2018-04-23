using Kompas6API5;
using Kompas6Constants3D;
using System;
using System.Runtime.InteropServices;

namespace FotoFrameModel
{
    /// <summary>
    /// Предоставляет возможность строить модель
    ///     по параметрам фоторамки в САПР
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Построить модель фоторамки в САПР
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки</param>
        /// <param name="checker">Проверяющий параметры фоторамки</param>
        /// <exception cref="InvalidOperationException">
        ///     Вызывается тогда, когда параметры фоторамки
        ///     имеют недопустимые значения.</exception>
        void Build(IPhotoFrame photoFrame, IChecker checker);
    }


    /// <summary>
    /// Мастер по созданию фоторамок в САПР Компас 3D
    /// </summary>
    public class BuilderPhotoFrame : IBuilder
    {
        private KompasObject _kompas;
        private const string progId = "KOMPAS.Application.5";

        private double _halfX;
        private double _halfY;

        public BuilderPhotoFrame()
        {
            ShowCAD();
        }

        private void RunCAD()
        {
            var kompasType =
               Type.GetTypeFromProgID(progId);

            try
            {
                //Получение ссылки на запущенную копию Компас 3д
                _kompas = (KompasObject)Marshal.
                    GetActiveObject(progId);
            }
            catch (COMException)
            {
                _kompas = (KompasObject)Activator.
                    CreateInstance(kompasType);
            }
        }

        private void ShowCAD()
        {
            var maxCount = 3;
            for (var i = 0; i < maxCount; i++)
            {
                try
                {
                    _kompas.Visible = true;
                }
                catch (Exception ex)
                    when (ex is COMException
                        || ex is NullReferenceException)
                {
                    RunCAD();
                }
            }
            _kompas?.ActivateControllerAPI();
        }

        /// <summary>
        /// Построить модель фоторамки в САПР Компас 3D
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки</param>
        /// <param name="checker">Проверяющий параметры фоторамки</param>
        /// <exception cref="InvalidOperationException">
        ///     Вызывается тогда, когда параметры фоторамки
        ///     имеют недопустимые значения.</exception>
        public void Build(IPhotoFrame photoFrame, IChecker checker)
        {
            if (checker.IsValid == false)
            {
                var msg = $"Шаблон фоторамки имеет" +
                    $" недопустимые параметры для построения.";
                throw new InvalidOperationException(msg);
            }

            ShowCAD();

            _halfX = 0;
            _halfY = _halfX;

            var doc = (ksDocument3D)_kompas.Document3D();
            doc.Create();

            GenerateBlock(photoFrame);
        }

        private void GenerateBlock(IPhotoFrame photoFrame)
        {
            var doc = (ksDocument3D)_kompas.ActiveDocument3D();
            if (doc == null)
            {
                return;
            }

            var part = (ksPart)doc.GetPart((short)Part_Type.pTop_Part);
            if (part == null)
            {
                return;
            }
            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);

            var sketch = (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketch == null)
            {
                return;
            }
            sketch.name = "Эскиз бруска стороны по длине";

            var sketchDef = (ksSketchDefinition)sketch.GetDefinition();
            if (sketchDef == null)
            {
                return;
            }
            sketchDef.SetPlane(basePlane);
            sketch.Create();

            var draw = (ksDocument2D)sketchDef.BeginEdit();

            var offsetX = 0;
            var offsetY = 0;
            var dx = photoFrame.OuterWidth;

            DrawSketchBlock(photoFrame, draw, offsetX, offsetY, dx);

            sketchDef.EndEdit();

            var extr = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_bossExtrusion);
            if (extr == null)
            {
                return;
            }
            extr.name = "Выдавить брусок";

            var extrDef = (ksBossExtrusionDefinition)extr.GetDefinition();
            extrDef.SetSketch(sketch);
            if (extrDef == null)
            {
                return;
            }

            extrDef.directionType = (short)Direction_Type.dtNormal;

            var depthExtrusion = photoFrame.OuterLength;
            var angle = 0.0f;
            extrDef.SetSideParam(true,
                                 (short)End_Type.etBlind,
                                 depthExtrusion,
                                 angle,
                                 false);
            extrDef.SetSketch(sketch);
            extr.Create();            

            GenerateOnWidthBlock(photoFrame, part);
        }

        private void GenerateOnWidthBlock(IPhotoFrame photoFrame, ksPart part)
        {
            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeYOZ);

            var sketch = (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketch == null)
            {
                return;
            }
            sketch.name = "Эскиз бруска стороны по ширине";

            var sketchDef = (ksSketchDefinition)sketch.GetDefinition();
            if (sketchDef == null)
            {
                return;
            }
            sketchDef.SetPlane(basePlane);
            sketch.Create();

            var draw = (ksDocument2D)sketchDef.BeginEdit();


            var offsetX = photoFrame.OuterLength;
            var offsetY = photoFrame.OuterHeight;
            var dx = photoFrame.OuterLength;

            DrawSketchBlock(photoFrame, draw, offsetX, offsetY, dx);

            sketchDef.EndEdit();

            var extr = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_bossExtrusion);
            if (extr == null)
            {
                return;
            }
            extr.name = "Выдавить брусок по ширине";

            var extrDef = (ksBossExtrusionDefinition)extr.GetDefinition();
            extrDef.SetSketch(sketch);
            if (extrDef == null)
            {
                return;
            }

            extrDef.directionType = (short)Direction_Type.dtReverse;

            var depthExtrusion = photoFrame.OuterWidth;
            var angle = 0.0f;
            extrDef.SetSideParam(false,
                                 (short)End_Type.etBlind,
                                 depthExtrusion,
                                 angle,
                                 false);
            extrDef.SetSketch(sketch);
            extr.Create();
        }

        /// <summary>
        /// Нарисовать эскиз двух зеркальных блоков для фоторамки
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки</param>
        /// <param name="draw">Документ эскиза</param>
        /// <param name="offsetX">Смещение по OX для начальной точки</param>
        /// <param name="offsetY">Смещение по OY для начальной точки</param>
        /// <param name="dx">Расстояние между начальными точками по OX
        ///     отрисовки брусков</param>
        private void DrawSketchBlock(IPhotoFrame photoFrame,
            ksDocument2D draw, double offsetX, double offsetY, double dx)
        {
            var h1 = photoFrame.OuterHeight;
            var h2 = photoFrame.InnerHeight;
            var h = photoFrame.Interval;

            var Ax = _halfX - offsetX;
            var Ay = _halfY - offsetY;

            var Bx = Ax;
            var By = Ay + h1;

            var h3 = (h1 - h2) / 2;

            var Dx = Ax + h;
            var Dy = Ay + h3;

            var Cx = Dx;
            var Cy = Dy + h2;

            var groupRightId = draw.ksNewGroup(0);

            draw.ksLineSeg(Ax, Ay, Bx, By, 1);
            draw.ksLineSeg(Bx, By, Cx, Cy, 1);
            draw.ksLineSeg(Cx, Cy, Dx, Dy, 1);
            draw.ksLineSeg(Dx, Dy, Ax, Ay, 1);

            draw.ksEndObj();

            Ax += dx / 2;
            Bx = Ax;

            //симметричное отображение трапеции
            draw.ksSymmetryObj(groupRightId, Ax, Ay, Bx, By, "1");
        }
    }
}
