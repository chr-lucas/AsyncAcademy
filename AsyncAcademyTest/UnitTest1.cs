namespace AsyncAcademyTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InstructorCanCreateCourseTest()
        {
            //Start with known instructor id.
            //Find out how many course that user is teaching = m
            //int m = SELECT COUNT(*) FROM course WHERE InstructorId = savedId;
            //Exercise the course creation functionality
            //Find out how many course that user is teaching = n
            //int m = SELECT COUNT(*) FROM course WHERE InstructorId = savedId;
            //Assert n = m + 1
            Assert.AreEqual((m+1), n);
        }
    }
}