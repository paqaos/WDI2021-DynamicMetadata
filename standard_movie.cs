class Recording {
	
}

class Person {

}
class Role : Person {
	
}

class Movie {
	List<Recording> Recordings {get;}
	Person Director {get;}
	List<Role> Actors {get;}
	List<Person> Producers {get;}
}