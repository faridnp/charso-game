using NUnit.Framework;
using UnityEngine;

namespace Charso.Tests
{
    public sealed class HafeziehChapterBuilderTests
    {
        [TearDown]
        public void TearDown()
        {
            var root=GameObject.Find("HafeziehChapter");
            if(root!=null)Object.DestroyImmediate(root);
            var camera=GameObject.Find("Main Camera");
            if(camera!=null)Object.DestroyImmediate(camera);
        }

        [Test]
        public void Build_CreatesTwentyColumnHallAndEightColumnPavilion()
        {
            HafeziehChapterBuilder.Build(0);
            var root=GameObject.Find("HafeziehChapter").transform;
            var hall=root.Find("Twenty column hall — 56m reference");
            var pavilion=root.Find("Eight column tomb pavilion");
            Assert.That(hall,Is.Not.Null);
            Assert.That(pavilion,Is.Not.Null);
            int hallColumns=0,pavilionColumns=0;
            foreach(Transform t in hall.GetComponentsInChildren<Transform>(true))
                if(t.name=="Hall stone column")hallColumns++;
            foreach(Transform t in pavilion.GetComponentsInChildren<Transform>(true))
                if(t.name=="Tomb pavilion column")pavilionColumns++;
            Assert.That(hallColumns,Is.EqualTo(20));
            Assert.That(pavilionColumns,Is.EqualTo(8));
            Assert.That(pavilion.gameObject.activeSelf,Is.False);
        }

        [Test]
        public void ReconstructionStage_EnablesPavilionAndDomeInSequence()
        {
            HafeziehChapterBuilder.Build(0);
            HafeziehChapterBuilder.ApplyStage(2);
            var root=GameObject.Find("HafeziehChapter").transform;
            Assert.That(root.Find("Eight column tomb pavilion").gameObject.activeSelf,Is.True);
            Assert.That(root.Find("ReconstructionStage_2").gameObject.activeSelf,Is.True);
            Assert.That(root.Find("Copper ribbed dome"),Is.Null);
        }
    }
}