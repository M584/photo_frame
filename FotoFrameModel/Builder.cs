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
        /// <exception cref="InvalidOperationException">
        ///     Вызывается тогда, когда параметры фоторамки
        ///     имеют недопустимые значения.</exception>
        void Build(IPhotoFrame photoFrame);
    }


    /// <summary>
    /// Мастер по созданию фоторамок в САПР Компас 3D
    /// </summary>
    public class BuilderPhotoFrame : IBuilder
    {
        /// <summary>
        /// Объект для связи с САПР Компас 3D
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// ID Компаса 3D в COM реестре
        /// </summary>
        private const string _progId = "KOMPAS.Application.5";

        /// <summary>
        /// Начальная координата по OX отрисовки для эскиза
        /// </summary>
        private double _startX;

        /// <summary>
        /// Начальная координата по OY отрисовки для эскиза
        /// </summary>
        private double _startY;

        /// <summary>
        /// Инициализация необходимых параметров для работы с Компас 3D
        /// </summary>
        public BuilderPhotoFrame()
        {
            ShowCAD();
        }

        /// <summary>
        /// Запуск Компаса 3D или подключение к запущенной версии
        /// </summary>
        private void RunCAD()
        {
            var kompasType =
               Type.GetTypeFromProgID(_progId);

            try
            {
                //Получение ссылки на запущенную копию Компас 3д
                _kompas = (KompasObject)Marshal.
                    GetActiveObject(_progId);
            }
            catch (COMException)
            {
                _kompas = (KompasObject)Activator.
                    CreateInstance(kompasType);
            }
        }

        /// <summary>
        /// Показывает окно Компаса 3D
        /// </summary>
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
        /// <exception cref="InvalidOperationException">
        ///     Вызывается тогда, когда параметры фоторамки
        ///     имеют недопустимые значения.</exception>
        public void Build(IPhotoFrame photoFrame)
        {
            if (photoFrame.IsValid == false)
            {
                var msg = $"Шаблон фоторамки имеет" +
                    $" недопустимые параметры для построения.";
                throw new InvalidOperationException(msg);
            }

            ShowCAD();

            _startX = 0;
            _startY = _startX;

            var doc = (ksDocument3D)_kompas.Document3D();
            doc.Create();

            var part = (ksPart)doc.GetPart((short)Part_Type.pTop_Part);
            if (part == null)
            {
                return;
            }

            GenerateBlockOnHeight(photoFrame, part);
            GenerateBlockOnWidth(photoFrame, part);
        }

        /// <summary>
        /// Создает модели брусков сторон фоторамки по внешней высоте
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки</param>
        /// <param name="part">Текущая сборка детали</param>
        private void GenerateBlockOnHeight(IPhotoFrame photoFrame,
            ksPart part)
        {
            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);

            var sketch = (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketch == null)
            {
                return;
            }
            sketch.name = "Эскиз брусков по длине";

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
            extr.name = "Выдавить бруски по длине";

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
        }

        /// <summary>
        /// Создает модели брусков сторон фоторамки по внешней высоте
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки</param>
        /// <param name="part">Текущая сборка детали</param>
        private void GenerateBlockOnWidth(IPhotoFrame photoFrame, ksPart part)
        {
            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeYOZ);

            var sketch = (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketch == null)
            {
                return;
            }
            sketch.name = "Эскиз брусков по ширине";

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
            extr.name = "Выдавить бруски по ширине";

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
        /// Нарисовать эскиз двух зеркальных блоков для фоторамки.
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки.</param>
        /// <param name="draw">Документ эскиза.</param>
        /// <param name="offsetX">Смещение по OX для начальной точки.</param>
        /// <param name="offsetY">Смещение по OY для начальной точки.</param>
        /// <param name="dx">Расстояние между начальными точками по OX
        ///     отрисовки брусков.</param>
        private void DrawSketchBlock(IPhotoFrame photoFrame,
            ksDocument2D draw, double offsetX, double offsetY, double dx)
        {
            var h1 = photoFrame.OuterHeight;
            var h2 = photoFrame.InnerHeight;
            var h = photoFrame.Interval;
            /* Блок для фоторамки представляет собой трапецию, 
             *      либо в частном случае квадрат.
             *          B
             *          |\h
             *          | \ C
             *      h1  |  | h2
             *          |  |
             *          | / D
             *          |/h
             *          A
            */

            //координаты для точки А
            var Ax = _startX - offsetX;
            var Ay = _startY - offsetY;
            //координаты для точки B
            var Bx = Ax;
            var By = Ay + h1;
            
            //
            var h3 = (h1 - h2) / 2;
            
            //координаты для точки D
            var Dx = Ax + h;
            var Dy = Ay + h3;
            //координаты для точки C
            var Cx = Dx;
            var Cy = Dy + h2;

            var groupRightId = draw.ksNewGroup(0);

            draw.ksLineSeg(Ax, Ay, Bx, By, 1);
            draw.ksLineSeg(Bx, By, Cx, Cy, 1);
            draw.ksLineSeg(Cx, Cy, Dx, Dy, 1);
            draw.ksLineSeg(Dx, Dy, Ax, Ay, 1);

            draw.ksEndObj();
            //координаты для зеркального отображения блока
            Ax += dx / 2;
            Bx = Ax;

            //симметричное отображение трапеции (блока)
            draw.ksSymmetryObj(groupRightId, Ax, Ay, Bx, By, "1");
        }
    }
}
