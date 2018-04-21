using System;

namespace FotoFrameModel
{
    public interface IBuilder
    {
        void Build(IPhotoFrame photoFrame, IChecker checker);
    }


    public class BuilderPhotoFrame : IBuilder
    {
        public void Build(IPhotoFrame photoFrame, IChecker checker)
        {
            if(checker.IsValid == false)
            {
                var msg = $"Шаблон фоторамки имеет" +
                    $" недопустимые параметры для построения.";
                throw new InvalidOperationException(msg);
            }
        }
    }
}
