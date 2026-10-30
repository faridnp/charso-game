using NUnit.Framework;
using UnityEngine;

namespace Charso.Tests
{
    public sealed class MehrganWorldBuilderTests
    {
        [TearDown]
        public void TearDown()
        {
            var world = GameObject.Find("MehrganWorld");
            if (world != null) Object.DestroyImmediate(world);
            var camera = GameObject.Find("Main Camera");
            if (camera != null) Object.DestroyImmediate(camera);
            var light = GameObject.Find("Mehrgan sun");
            if (light != null) Object.DestroyImmediate(light);
        }

        [Test]
        public void Build_CreatesNamedDistrictsAndCamera()
        {
            MehrganWorldBuilder.Build(0);

            Assert.That(GameObject.Find("MehrganWorld"), Is.Not.Null);
            Assert.That(GameObject.Find("Covered bazaar"), Is.Not.Null);
            Assert.That(GameObject.Find("Chahar Bagh Persian garden"), Is.Not.Null);
            Assert.That(GameObject.Find("West residential quarter"), Is.Not.Null);
            Assert.That(GameObject.Find("Civic courtyard"), Is.Not.Null);
            Assert.That(Camera.main, Is.Not.Null);
        }

        [Test]
        public void RestorationStage_ActivatesOnlySelectedLandmarkVariant()
        {
            MehrganWorldBuilder.Build(0);
            MehrganWorldBuilder.ApplyRestorationStage(2);

            var site = GameObject.Find("MehrganLandmarkSite").transform;
            Assert.That(site.Find("RestorationStage_0").gameObject.activeSelf, Is.False);
            Assert.That(site.Find("RestorationStage_1").gameObject.activeSelf, Is.False);
            Assert.That(site.Find("RestorationStage_2").gameObject.activeSelf, Is.True);
            Assert.That(site.Find("RestorationStage_3").gameObject.activeSelf, Is.False);
        }
    }
}