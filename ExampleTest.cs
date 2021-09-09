using testify;

class ExampleTest
{
    static void Main(string[] args)
    {

        var suite = new TestSuite();
        suite.Describe("Pass suite - BasicTypes", () =>
        {
            suite.It("should pass on Boolean comparison", (assert) => assert(true, true));
            suite.It("should pass on int comparison", (assert) => assert(1, 1));
            suite.It("should pass on string comparison", (assert) => assert("foo", "foo"));
        });
        
        suite.Describe("Failure, Excluded and NotImplemented suite - BasicTypes", () =>
        {
            suite.It("should fail on Boolean comparison", (assert) => assert(true, false));
            suite.It("should fail on int comparison", (assert) => assert(-1, 1));
            suite.It("should fail on string comparison", (assert) => assert("foo", "bar"));
            
            suite.XIt("demonstrate an example of excluded test", (assert) => assert(true, true));
            
            suite.It("demonstrate an example of not implemented test");
        });

        suite.Wait();

    }
}