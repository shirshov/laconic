using Xunit;

namespace Laconic.CodeGen.Tests
{
    // ReSharper disable UnusedType.Global
    // ReSharper disable UnusedMember.Global
    [Records]
    public interface TestRecords
    {
        record User(string firstName, string lastName);
        record TaggedUser(User user, params string[] tags);
    }
    // ReSharper restore UnusedType.Global
    // ReSharper restore UnusedMember.Global

    public class RecordTests
    {
        [Fact]
        public void parameters_become_property_names()
        {
            var user = new User("a", "b");

            Assert.Equal("a", user.FirstName);
            Assert.Equal("b", user.LastName);
        }

        [Fact]
        public void With_method_works()
        {
            var user = new User("a", "b");
            var updated = user.With(lastName: "c");

            Assert.Equal("a", updated.FirstName);
            Assert.Equal("c", updated.LastName);
        }

        [Fact]
        public void Deconstruct_method_works()
        {
            var user = new User("a", "b");
            var (first, last) = user;

            Assert.Equal("a", first);
            Assert.Equal("b", last);
        }

        [Fact]
        public void Records_have_structural_equality()
        {
            Assert.Equal(new User("a", "b"), new User("a", "b"));
            Assert.True(((object) new User("a", "b")).Equals((object) new User("a", "b")));

            Assert.NotEqual(new User("a", "b"), new User("a", "c"));

            Assert.True(new User("a", "b") == new User("a", "b"));
            Assert.False(new User("a", "b") == new User("a", "c"));
        }

        [Fact]
        public void Records_define_GetHashCode()
        {
            var user1 = new User("a", "b");
            var user2 = new User("a", "b");
            var user3 = new User("a", "c");

            Assert.Equal(user1.GetHashCode(), user2.GetHashCode());
            Assert.NotEqual(user1.GetHashCode(), user3.GetHashCode());
        }

        [Fact]
        public void Records_with_params_arrays_have_structural_equality()
        {
            Assert.Equal(
                new TaggedUser(new User("a", "b"), "t1", "t2"),
                new TaggedUser(new User("a", "b"), "t1", "t2"));

            Assert.NotEqual(
                new TaggedUser(new User("a", "b"), "t1", "t2"),
                new TaggedUser(new User("a", "b"), "t1", "t2", "t3"));
        }
    }
}