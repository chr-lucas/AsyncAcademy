from tests import *

def runTest(test, name):
    result = test()
    if result[0] == True:
        print("Test passed :", name)
    else:
        print("TEST FAILED :", name)
        print(result[1])
        print(result[2])

runTest(testAccountCreation, "Account Creation")
runTest(testGraphVisibility, "Graph Visibility for Student/Instructor Views")
runTest(testUpdateProfile, "Profile update functionality")
runTest(testCourseRegistration, "Course registration functionality")
runTest(testViewToDoList, "To do list visibility")
runTest(testAssignmentCreation, "Assingment Creation")

