QUEST_HERO = {
	title = 'IDS_PROPQUEST_INC_002064',
	character = 'MaDa_RedRobeGirl',
	end_character = 'MaDa_RedRobeGirl',
	start_requirements = {
		min_level = 120,
		max_level = 120,
		job = { 'JOB_KNIGHT_MASTER', 'JOB_BLADE_MASTER', 'JOB_JESTER_MASTER', 'JOB_RANGER_MASTER', 'JOB_RINGMASTER_MASTER', 'JOB_BILLPOSTER_MASTER', 'JOB_PSYCHIKEEPER_MASTER', 'JOB_ELEMENTOR_MASTER' },
		previous_quest = '',
	},
	rewards = {
		gold = 0,
	},
	end_conditions = {
		items = {
			{ id = 'II_GEN_GEM_GEM_DRAGONHEART', quantity = 5, sex = 'Any', remove = true },
			{ id = 'II_GEN_GEM_GEM_DRAGONCANINE', quantity = 10, sex = 'Any', remove = true },
			{ id = 'II_GEN_GEM_GEM_STRANGEEYES', quantity = 10, sex = 'Any', remove = true },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_002065',
			'IDS_PROPQUEST_INC_002066',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_002067',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_002068',
		},
		completed = {
			'IDS_PROPQUEST_INC_002069',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_002070',
		},
	}
}
