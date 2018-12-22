import java.util.Set;

/**
 * Interface Map represents an abstract data type (ADT) for a map storing
 * a collection of key-value pairs.
 * A map allows efficient insertion, removal, and lookup of pairs by keys.
 * 
 * Classes implementing Map are generally expected to provide the operations
 * below in O(1) or O(log N) average runtime, except for the clear, keySet,
 * and toString operations, which are often O(N).
 */
public interface Map<K, V> {
	// (Comments are intentionally omitted from these abstract method headings,
	//  because we want you to write comments for these methods in your own
	//  words in your HashMap class.)
	
	/** @see Homework spec for details */
	void clear();

	/** @see Homework spec for details */
	boolean containsKey(K key);

	/** @see Homework spec for details */
	V get(K key);
	
	/** @see Homework spec for details */
	boolean isEmpty();
	
	/** @see Homework spec for details */
	Set<K> keySet();
	
	/** @see Homework spec for details */
	void put(K key, V value);
	
	/** @see Homework spec for details */
	void remove(K key);
	
	/** @see Homework spec for details */
	int size();
	
	/** @see Homework spec for details */
	String toString();
}
