from tests import *

def runTest(test, name):
    result = test()
    if result[0] == True:
        print("Test passed :", name)
    else:
        print("TEST FAILED :", name)
        print(result[1])
        print(result[2])

runTest(testLogIn, "Login") #PASSED
runTest(testAccountCreation, "Account Creation") #Failed
runTest(testGraphVisibility, "Graph Visibility for Student/Instructor Views") #Failed
runTest(testUpdateProfile, "Profile update functionality") #PASSED
runTest(testCourseRegistration, "Course registration functionality") #FAILED
runTest(testViewToDoList, "To do list visibility") #PASSED
runTest(testAssignmentCreation, "Assingment Creation") #Failed

