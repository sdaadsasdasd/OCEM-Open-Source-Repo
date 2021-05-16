# List of maintainers and developers and how to submit changes

### The hierarchy in this project is as follows:

1. Maintainers: The final testers of features who will merge all 
                changes into the master branch. They are long
                time developers who have stuck with the project
                and know a lot about it inside-out.

2. Developers:  The creators of features. They will be replaced more
                often as people come and join.

### Please try to follow the guidelines below.  This will make things easier on the maintainers.  Not all of these guidelines matter for every trivial patch so apply some common sense. These are mostly for major changes.

1.	Always _test_ your changes, however small, on at least 4 or
	5 people, preferably many more.

2.	Try to release a few ALPHA test versions to your own branches before RED merges 
    them into master. Announce them onto the discord channel and await results. 
    This is especially  important for  super-specific game mechanics, because often 
    that's the only way you will find things like the fact an older dependency script needs
	a magic fix you didn't know about, or some clown changed the names of the public
	variables in a supporting script and not its name. (Don't laugh! It happens all the time.)

3.	Make sure your changes compile correctly in multiple
	configurations. If working on multiplayer, try playing 
    the game on your local network with yourself.

4.	When you are happy with a major change make it generally available for
	testing and await feedback.

5.	Make a patch available to the relevant maintainer in the list. Use
	'diff -u' to make the patch easy to merge. Be prepared to get your
	changes sent back with seemingly silly requests about formatting
	and variable names.  These aren't as silly as they seem. One
	job the maintainers (and especially Red) do is to keep things
	looking the same. Sometimes this means that the clever hack in
	your code to get around a problem actually needs to become a
	generalized feature ready for next time the problem re-appears.

	PLEASE document known bugs. If it doesn't work for everything
	or does something very odd once a month document it.

6.	When sending bug-related changes or reports to a maintainer
	please Cc: *reds email*, especially if the maintainer
	does not respond. Please keep in mind that the maintainers team is
	a small set of people who can be efficient only when working on
	verified bugs.

7.  Make sure to go through the [formatting guide](https://github.com/RedEagleP1/connect-ed/blob/main/FORMATTING.md) This ensures 
    that your code is readable for future developers and maintainers

8.	Happy coding.

### Maintainers List (try to look for most precise areas first)

		        -----------------------------------

HEAD MAINTAINER
Red
email

Maintainers to be updated...
