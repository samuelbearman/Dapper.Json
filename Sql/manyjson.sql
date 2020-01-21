select P.*,
	json_query((
		select C.*
		from Children C
		where P.ParentId = C.ParentId
		for json path)) as [Children],
	json_query((
		select S.*
		from Siblings S
		where P.SiblingId = S.SiblingId
		for json path, without_array_wrapper)) as [Sibling]
	from Parents P
	for json path, without_array_wrapper